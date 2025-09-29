using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.DAL.DTO;
using BE.DAL.GenericRepository;
using BE.DAL.Models;
using BE.DAL.UOW;
using Microsoft.EntityFrameworkCore;

namespace BE.Services.Services.Implemetation
{
    public class VehicleService : BaseService, IVehicleService
    {
        private IGenericRepository<Vehicle> _vehicleRepo;
        private IUnitOfWork _unitOfWork;

        public VehicleService(IServiceProvider serviceProvider, IGenericRepository<Vehicle> vehicleRepo, IUnitOfWork unitOfWork) : base(serviceProvider)
        {
            _vehicleRepo = vehicleRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Part> AddPartIntoVehicle(Guid vehicleId, PartDTO part)
        {
            var vehicle = await _vehicleRepo.GetById(vehicleId);
            if (vehicle == null)
            {
                throw new Exception("Vehicle not found");
            }
            Part entity = new Part();
            entity.Id = Guid.NewGuid();
            entity.Name = part.Name;
            entity.SerialNumber = part.SerialNumber;
            entity.VehicleId = vehicleId;
            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                var partRepo = Resolve<IGenericRepository<Part>>();
                await partRepo.Insert(entity);

            });
            await _unitOfWork.SaveChangesAsync();
            return entity;

        }

        public async Task<Part> EditPartVehicle(PartDTO part)
        {
            if (part == null || part.Id == Guid.Empty)
                throw new ArgumentException("Invalid part data");

            var partRepo = Resolve<IGenericRepository<Part>>();
            var existingPart = await partRepo.GetById(part.Id);

            if (existingPart == null)
                throw new Exception("Part not found");

            existingPart.Name = part.Name;
            existingPart.SerialNumber = part.SerialNumber;

            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                await partRepo.Update(existingPart);
            });

            await _unitOfWork.SaveChangesAsync();
            return existingPart;
        }


        public async Task<Part> RemovePartVehicle(Guid partId)
        {
            if (partId == Guid.Empty)
                throw new ArgumentException("Invalid part Id");

            var partRepo = Resolve<IGenericRepository<Part>>();
            var existingPart = await partRepo.GetById(partId);

            if (existingPart == null)
                throw new Exception("Part not found");

            await partRepo.DeleteById(partId);

            await _unitOfWork.SaveChangesAsync();
            return existingPart;
        }

        // New DTO-based CRUD methods
        public async Task<PagedResult<VehicleWithDetailsDTO>> GetAllVehiclesAsync()
        {
            var vehiclesResult = await _vehicleRepo.GetAllDataByExpression(new QueryOptions<Vehicle>
            {
                Filter = null,
                PageNumber = 1,
                PageSize = int.MaxValue,
                Includes = new List<Func<IQueryable<Vehicle>, IQueryable<Vehicle>>>
                {
                    q => q.Select(v => v)
                        .Include(v => v.Customer)
                        .Include(v => v.Parts)
                        .Include(v => v.ServiceHistories)
                        .Include(v => v.WarrantyClaims)
                }
            });

            // Convert to DTO to avoid circular reference
            var vehicleDTOs = vehiclesResult.Items.Select(v => new VehicleWithDetailsDTO
            {
                Id = v.Id,
                VIN = v.VIN,
                VehicleName = v.VehicleName,
                CustomerId = v.CustomerId,
                Customer = v.Customer != null ? new CustomerDTO
                {
                    Id = v.Customer.Id,
                    Name = v.Customer.Name,
                    Email = v.Customer.Email,
                    Phone = v.Customer.Phone,
                    Address = v.Customer.Address
                } : null,
                Parts = v.Parts?.Select(p => new PartDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    SerialNumber = p.SerialNumber
                }).ToList(),
                ServiceHistories = v.ServiceHistories?.Select(sh => new ServiceHistoryDTO
                {
                    Id = sh.Id,
                    ServiceDate = sh.ServiceDate,
                    Description = sh.Description,
                    Technician = sh.Technician
                }).ToList(),
                WarrantyClaims = v.WarrantyClaims?.Select(wc => new WarrantyClaimDTO
                {
                    Id = wc.Id,
                    ClaimDate = wc.ClaimDate,
                    Status = wc.Status,
                    Report = wc.Report,
                    DiagnosticInfo = wc.DiagnosticInfo,
                    ImageUrls = wc.ImageUrls
                }).ToList()
            }).ToList();

            return new PagedResult<VehicleWithDetailsDTO>
            {
                Items = vehicleDTOs,
                TotalItems = vehiclesResult.TotalItems,
                PageSize = vehiclesResult.PageSize,
                TotalPages = vehiclesResult.TotalPages
            };
        }

        public async Task<PagedResult<Part>> GetVehicleByIdAsync(Guid id)
        {
            var repository = Resolve<IGenericRepository<Part>>();
            var data = await repository.GetAllDataByExpression(new QueryOptions<Part>
            {
                Filter = s => s.VehicleId == id,
                PageNumber = 1,
                PageSize = int.MaxValue
            });
            return data;
        }

        public async Task<Vehicle> CreateVehicleAsync(VehicleDTO vehicleDto)
        {
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                VIN = vehicleDto.VIN,
                VehicleName = vehicleDto.VehicleName,
                CustomerId = (Guid)vehicleDto.CustomerId
            };

            await _vehicleRepo.Insert(vehicle);
            await _unitOfWork.SaveChangesAsync();

            return vehicle;
        }

        public async Task<Vehicle> UpdateVehicleAsync(VehicleDTO vehicleDto)
        {
            if (vehicleDto.Id == null)
                throw new ArgumentException("Vehicle ID is required for update");

            var existingVehicle = await _vehicleRepo.GetById(vehicleDto.Id);
            if (existingVehicle == null)
                throw new Exception("Vehicle not found");

            existingVehicle.VIN = vehicleDto.VIN;
            existingVehicle.VehicleName = vehicleDto.VehicleName;
            existingVehicle.CustomerId = (Guid)vehicleDto.CustomerId;

            await _vehicleRepo.Update(existingVehicle);
            await _unitOfWork.SaveChangesAsync();

            return existingVehicle;
        }

        public async Task<bool> DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _vehicleRepo.GetByExpression(s=> s.Id==id, null);
            if (vehicle == null)
                return false;

            await _vehicleRepo.Delete(s=> s.Id == id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

    }
}
