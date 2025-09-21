using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Common;
using BE.DAL.DTO;
using BE.DAL.GenericRepository;
using BE.DAL.Models;
using BE.DAL.UOW;
using Microsoft.EntityFrameworkCore;

namespace BE.Services.Services.Implemetation
{
    public class ServiceHistoryService : BaseService, IServiceHistoryService
    {
        private readonly IGenericRepository<ServiceHistory> _serviceHistoryRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceHistoryService(
            IServiceProvider serviceProvider,
            IGenericRepository<ServiceHistory> serviceHistoryRepo,
            IUnitOfWork unitOfWork
        ) : base(serviceProvider)
        {
            _serviceHistoryRepo = serviceHistoryRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceHistory> AddServiceHistory(ServiceHistory history)
        {
            history.Id = Guid.NewGuid();
            history.ServiceDate = history.ServiceDate == default ? DateTime.UtcNow : history.ServiceDate;

            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                await _serviceHistoryRepo.Insert(history);
            });

            await _unitOfWork.SaveChangesAsync();
            return history;
        }

        public async Task<ServiceHistory> EditServiceHistory(ServiceHistory history)
        {
            var existing = await _serviceHistoryRepo.GetById(history.Id);
            if (existing == null)
                throw new Exception("Service history not found");

            existing.ServiceDate = history.ServiceDate;
            existing.Description = history.Description;
            existing.Technician = history.Technician;

            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                await _serviceHistoryRepo.Update(existing);
            });

            await _unitOfWork.SaveChangesAsync();
            return existing;
        }

        public async Task<ServiceHistory> RemoveServiceHistory(Guid id)
        {
            var existing = await _serviceHistoryRepo.GetById(id);
            if (existing == null)
                throw new Exception("Service history not found");
            await _serviceHistoryRepo.DeleteById(id);

            await _unitOfWork.SaveChangesAsync();
            return existing;
        }

        public async Task<PagedResult<ServiceHistory>> GetByVehicleId(Guid vehicleId)
        {
            return await _serviceHistoryRepo.GetAllDataByExpression( new DAL.DTO.QueryOptions<ServiceHistory>
            {
                Filter = sh => sh.VehicleId == vehicleId,
               
                PageNumber = 1,
                PageSize = int.MaxValue
            });
        }

        public async Task<ServiceHistory> GetById(Guid id)
        {
            return await _serviceHistoryRepo.GetByExpression( s=> s.Id == id, new List<Func<IQueryable<ServiceHistory>, IQueryable<ServiceHistory>>>
            {
                s=> s.Include(s=> s.Vehicle)
            });
        }

        // New DTO-based methods
        public async Task<IEnumerable<ServiceHistory>> GetAllServiceHistoriesAsync()
        {
            return await _serviceHistoryRepo.GetAll();
        }

        public async Task<ServiceHistory> GetServiceHistoryByIdAsync(Guid id)
        {
            return await _serviceHistoryRepo.GetById(id);
        }

        public async Task<ServiceHistory> CreateServiceHistoryAsync(ServiceHistoryDTO historyDto)
        {
            var history = new ServiceHistory
            {
                Id = Guid.NewGuid(),
                VehicleId = historyDto.VehicleId,
                ServiceDate = historyDto.ServiceDate == default ? DateTime.UtcNow : historyDto.ServiceDate,
                Description = historyDto.Description,
                Technician = historyDto.Technician
            };

            await _serviceHistoryRepo.Insert(history);
            await _unitOfWork.SaveChangesAsync();
            
            return history;
        }

        public async Task<ServiceHistory> UpdateServiceHistoryAsync(ServiceHistoryDTO historyDto)
        {
            if (historyDto.Id == null)
                throw new ArgumentException("Service history ID is required for update");

            var existingHistory = await _serviceHistoryRepo.GetById(historyDto.Id.Value);
            if (existingHistory == null)
                throw new Exception("Service history not found");

            existingHistory.VehicleId = historyDto.VehicleId;
            existingHistory.ServiceDate = historyDto.ServiceDate;
            existingHistory.Description = historyDto.Description;
            existingHistory.Technician = historyDto.Technician;

            await _serviceHistoryRepo.Update(existingHistory);
            await _unitOfWork.SaveChangesAsync();

            return existingHistory;
        }

        public async Task<bool> DeleteServiceHistoryAsync(Guid id)
        {
            var history = await _serviceHistoryRepo.GetById(id);
            if (history == null)
                return false;

            await _serviceHistoryRepo.Delete(history);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<ServiceHistory>> GetByVehicleIdAsync(Guid vehicleId)
        {
            return await _serviceHistoryRepo.GetAllDataByExpression(new DAL.DTO.QueryOptions<ServiceHistory>
            {
                Filter = sh => sh.VehicleId == vehicleId,
                PageNumber = 1,
                PageSize = int.MaxValue
            });
        }
    }
}
