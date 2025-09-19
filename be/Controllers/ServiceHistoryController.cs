using ElectricVehicleWarranty.Data;
using ElectricVehicleWarranty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectricVehicleWarranty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceHistoryController : ControllerBase
    {
        private readonly WarrantyDbContext _context;
        public ServiceHistoryController(WarrantyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var histories = await _context.ServiceHistories.ToListAsync();
            return Ok(histories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceHistory history)
        {
            history.Id = Guid.NewGuid();
            _context.ServiceHistories.Add(history);
            await _context.SaveChangesAsync();
            return Ok(history);
        }
    }
}
