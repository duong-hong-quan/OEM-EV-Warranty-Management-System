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

namespace BE.Services.Services.Implemetation
{
    public class PartService : BaseService, IPartService
    {
        private IGenericRepository<Part> _partRepo;
        private IUnitOfWork _unitOfWork;

        public PartService(IServiceProvider serviceProvider, IGenericRepository<Part> partRepo, IUnitOfWork unitOfWork) : base(serviceProvider)
        {
            _partRepo = partRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Part>> GetAllPartsAsync()
        {
            return await _partRepo.GetAllDataByExpression(new DAL.DTO.QueryOptions<Part>
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
            });
        }

        public async Task<Part> GetPartByIdAsync(Guid id)
        {
            return await _partRepo.GetById(id);
        }

        public async Task<Part> CreatePartAsync(PartDTO partDto)
        {
            var part = new Part
            {
                Id = Guid.NewGuid(),
                SerialNumber = partDto.SerialNumber,
                Name = partDto.Name,
                VehicleId = partDto.VehicleId
            };

            await _partRepo.Insert(part);
            await _unitOfWork.SaveChangesAsync();
            
            return part;
        }

        public async Task<Part> UpdatePartAsync(PartDTO partDto)
        {
            if (partDto.Id == null)
                throw new ArgumentException("Part ID is required for update");

            var existingPart = await _partRepo.GetById(partDto.Id.Value);
            if (existingPart == null)
                throw new Exception("Part not found");

            existingPart.SerialNumber = partDto.SerialNumber;
            existingPart.Name = partDto.Name;
            existingPart.VehicleId = partDto.VehicleId;

            await _partRepo.Update(existingPart);
            await _unitOfWork.SaveChangesAsync();

            return existingPart;
        }

        public async Task<bool> DeletePartAsync(Guid id)
        {
            var part = await _partRepo.GetById(id);
            if (part == null)
                return false;

            await _partRepo.DeleteById(part);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
