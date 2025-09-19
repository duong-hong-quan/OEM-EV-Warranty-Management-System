using ElectricVehicleWarranty.Data;
using ElectricVehicleWarranty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectricVehicleWarranty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarrantyClaimsController : ControllerBase
    {
        private readonly WarrantyDbContext _context;
        public WarrantyClaimsController(WarrantyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var claims = await _context.WarrantyClaims.ToListAsync();
            return Ok(claims);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WarrantyClaim claim)
        {
            claim.Id = Guid.NewGuid();
            claim.ClaimDate = DateTime.UtcNow;
            claim.Status = "Sent";
            _context.WarrantyClaims.Add(claim);
            await _context.SaveChangesAsync();
            return Ok(claim);
        }
    }
}
