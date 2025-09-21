using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.DAL.Models;

namespace BE.Services.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle> GetVehicleByIdAsync(Guid id);
        Task<Vehicle> CreateVehicleAsync(VehicleDTO vehicleDto);
        Task<Vehicle> UpdateVehicleAsync(VehicleDTO vehicleDto);
        Task<bool> DeleteVehicleAsync(Guid id);
        
        // Existing part-related methods
        Task<Part> AddPartIntoVehicle(Guid vehicleId, PartDTO part);
        Task<Part> RemovePartVehicle(Guid partId);
        Task<Part> EditPartVehicle(PartDTO part);
    }
}
