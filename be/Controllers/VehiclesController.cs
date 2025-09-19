using ElectricVehicleWarranty.Data;
using ElectricVehicleWarranty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectricVehicleWarranty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly WarrantyDbContext _context;
        public VehiclesController(WarrantyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _context.Vehicles.Include(v => v.Parts).Include(v => v.ServiceHistories).Include(v => v.WarrantyClaims).ToListAsync();
            return Ok(vehicles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            vehicle.Id = Guid.NewGuid();
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return Ok(vehicle);
        }
    }
}
