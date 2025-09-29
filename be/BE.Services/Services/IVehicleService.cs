using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.DAL.DTO;
using BE.DAL.Models;

namespace BE.Services.Services
{
    public interface IVehicleService
    {
        Task<PagedResult<VehicleWithDetailsDTO>> GetAllVehiclesAsync();
         Task<PagedResult<Part>> GetVehicleByIdAsync(Guid id);
        Task<Vehicle> CreateVehicleAsync(VehicleDTO vehicleDto);
        Task<Vehicle> UpdateVehicleAsync(VehicleDTO vehicleDto);
        Task<bool> DeleteVehicleAsync(Guid id);
        
        // Existing part-related methods
        Task<Part> AddPartIntoVehicle(Guid vehicleId, PartDTO part);
        Task<Part> RemovePartVehicle(Guid partId);
        Task<Part> EditPartVehicle(PartDTO part);
    }
}
