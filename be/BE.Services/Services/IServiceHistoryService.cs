using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Common;
using BE.DAL.DTO;
using BE.DAL.Models;

namespace BE.Services.Services
{
    public interface IServiceHistoryService
    {
        Task<IEnumerable<ServiceHistory>> GetAllServiceHistoriesAsync();
        Task<ServiceHistory> GetServiceHistoryByIdAsync(Guid id);
        Task<ServiceHistory> CreateServiceHistoryAsync(ServiceHistoryDTO historyDto);
        Task<ServiceHistory> UpdateServiceHistoryAsync(ServiceHistoryDTO historyDto);
        Task<bool> DeleteServiceHistoryAsync(Guid id);
        Task<PagedResult<ServiceHistory>> GetByVehicleIdAsync(Guid vehicleId);
        
        // Keep existing methods for backward compatibility
        Task<ServiceHistory> AddServiceHistory(ServiceHistory history);
        Task<ServiceHistory> EditServiceHistory(ServiceHistory history);
        Task<ServiceHistory> RemoveServiceHistory(Guid id);
        Task<PagedResult<ServiceHistory>> GetByVehicleId(Guid vehicleId);
        Task<ServiceHistory> GetById(Guid id);
    }
}
