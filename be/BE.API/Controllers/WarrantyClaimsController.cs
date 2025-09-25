using BE.Common;
using BE.DAL.Models;
using BE.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.API.Controllers
{
    [ApiController]
    [Route("api/warranty-claim")]
    [Authorize] // Require authentication
    public class WarrantyClaimsController : ControllerBase
    {
        private readonly IWarrantyClaimService _warrantyClaimService;
        
        public WarrantyClaimsController(IWarrantyClaimService warrantyClaimService)
        {
            _warrantyClaimService = warrantyClaimService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")] // Only staff can see all claims
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var claims = await _warrantyClaimService.GetAllWarrantyClaimsAsync();
                return Ok(claims);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var claim = await _warrantyClaimService.GetWarrantyClaimByIdAsync(id);
                if (claim == null)
                    return NotFound();
                return Ok(claim);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(WarrantyClaimDTO claimDto)
        {
            try
            {
                var claim = await _warrantyClaimService.CreateWarrantyClaimAsync(claimDto);
                return CreatedAtAction(nameof(GetById), new { id = claim.Id }, claim);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(WarrantyClaimDTO claimDto)
        {
            try
            {
                var claim = await _warrantyClaimService.UpdateWarrantyClaimAsync(claimDto);
                return Ok(claim);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _warrantyClaimService.DeleteWarrantyClaimAsync(id);
                if (!success)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicleId(Guid vehicleId)
        {
            try
            {
                var result = await _warrantyClaimService.GetByVehicleIdAsync(vehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
