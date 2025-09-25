using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Common;
using BE.DAL.DTO;
using BE.DAL.GenericRepository;
using BE.DAL.Models;
using BE.DAL.UOW;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BE.Services.Services.Implemetation
{
    public class WarrantyClaimService : BaseService, IWarrantyClaimService
    {
        private readonly IGenericRepository<WarrantyClaim> _claimRepo;
        private readonly IUnitOfWork _unitOfWork;

        public WarrantyClaimService(IServiceProvider serviceProvider, IGenericRepository<WarrantyClaim> claimRepo, IUnitOfWork unitOfWork)
            : base(serviceProvider)
        {
            _claimRepo = claimRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<WarrantyClaim> AddClaim(WarrantyClaim claim)
        {
            claim.Id = Guid.NewGuid();
            claim.ClaimDate = DateTime.UtcNow;

            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                await _claimRepo.Insert(claim);
            });

            await _unitOfWork.SaveChangesAsync();
            return claim;
        }

        public async Task<WarrantyClaim> EditClaim(WarrantyClaim claim)
        {
            var existing = await _claimRepo.GetById(claim.Id);
            if (existing == null)
                throw new Exception("Warranty claim not found");

            existing.Status = claim.Status;
            existing.Report = claim.Report;
            existing.DiagnosticInfo = claim.DiagnosticInfo;
            existing.ImageUrls = claim.ImageUrls;

            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                await _claimRepo.Update(existing);
            });

            await _unitOfWork.SaveChangesAsync();
            return existing;
        }

        public async Task<WarrantyClaim> RemoveClaim(Guid id)
        {
            var existing = await _claimRepo.GetById(id);
            if (existing == null)
                throw new Exception("Warranty claim not found");

            await _claimRepo.DeleteById(id);


            await _unitOfWork.SaveChangesAsync();
            return existing;
        }

        public async Task<PagedResult<WarrantyClaim>> GetByVehicleId(Guid vehicleId)
        {
            return await _claimRepo.GetAllDataByExpression(new DAL.DTO.QueryOptions<WarrantyClaim>
            {
                Filter = c => c.VehicleId == vehicleId,
                PageNumber = 1,
                PageSize = int.MaxValue
            });
        }

        // New DTO-based methods
        public async Task<PagedResult<WarrantyClaim>> GetAllWarrantyClaimsAsync()
        {
            return await _claimRepo.GetAllDataByExpression(
                new QueryOptions<WarrantyClaim>
                {
                    PageNumber = 1,
                    PageSize = int.MaxValue
                });
        }

        public async Task<WarrantyClaim> GetWarrantyClaimByIdAsync(Guid id)
        {
            return await _claimRepo.GetById(id);
        }

        public async Task<WarrantyClaim> CreateWarrantyClaimAsync(WarrantyClaimDTO claimDto)
        {
            var claim = new WarrantyClaim
            {
                Id = Guid.NewGuid(),
                VehicleId = claimDto.VehicleId,
                ClaimDate = claimDto.ClaimDate ?? DateTime.UtcNow,
                Status = claimDto.Status ?? "Sent",
                Report = claimDto.Report,
                DiagnosticInfo = claimDto.DiagnosticInfo,
                ImageUrls = claimDto.ImageUrls ?? new List<string>()
            };

            await _claimRepo.Insert(claim);
            await _unitOfWork.SaveChangesAsync();
            
            return claim;
        }

        public async Task<WarrantyClaim> UpdateWarrantyClaimAsync(WarrantyClaimDTO claimDto)
        {
            if (claimDto.Id == null)
                throw new ArgumentException("Warranty claim ID is required for update");

            var existingClaim = await _claimRepo.GetById(claimDto.Id.Value);
            if (existingClaim == null)
                throw new Exception("Warranty claim not found");

            existingClaim.VehicleId = claimDto.VehicleId;
            if (claimDto.ClaimDate.HasValue)
                existingClaim.ClaimDate = claimDto.ClaimDate.Value;
            existingClaim.Status = claimDto.Status ?? existingClaim.Status;
            existingClaim.Report = claimDto.Report;
            existingClaim.DiagnosticInfo = claimDto.DiagnosticInfo;
            existingClaim.ImageUrls = claimDto.ImageUrls ?? existingClaim.ImageUrls;

            await _claimRepo.Update(existingClaim);
            await _unitOfWork.SaveChangesAsync();

            return existingClaim;
        }

        public async Task<bool> DeleteWarrantyClaimAsync(Guid id)
        {
            var claim = await _claimRepo.GetById(id);
            if (claim == null)
                return false;

            await _claimRepo.DeleteById(claim);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<WarrantyClaim>> GetByVehicleIdAsync(Guid vehicleId)
        {
            return await _claimRepo.GetAllDataByExpression(new DAL.DTO.QueryOptions<WarrantyClaim>
            {
                Filter = c => c.VehicleId == vehicleId,
                PageNumber = 1,
                PageSize = int.MaxValue
            });
        }

    }
}
