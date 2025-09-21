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
    public interface IWarrantyClaimService
    {
        Task<IEnumerable<WarrantyClaim>> GetAllWarrantyClaimsAsync();
        Task<WarrantyClaim> GetWarrantyClaimByIdAsync(Guid id);
        Task<WarrantyClaim> CreateWarrantyClaimAsync(WarrantyClaimDTO claimDto);
        Task<WarrantyClaim> UpdateWarrantyClaimAsync(WarrantyClaimDTO claimDto);
        Task<bool> DeleteWarrantyClaimAsync(Guid id);
        Task<PagedResult<WarrantyClaim>> GetByVehicleIdAsync(Guid vehicleId);
        
        // Keep existing methods for backward compatibility
        Task<WarrantyClaim> AddClaim(WarrantyClaim claim);
        Task<WarrantyClaim> EditClaim(WarrantyClaim claim);
        Task<WarrantyClaim> RemoveClaim(Guid id);
        Task<PagedResult<WarrantyClaim>> GetByVehicleId(Guid vehicleId);
    }
}
}
