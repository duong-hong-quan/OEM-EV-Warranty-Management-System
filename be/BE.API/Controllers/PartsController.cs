using BE.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartsController : ControllerBase
    {
        private readonly WarrantyDbContext _context;
        public PartsController(WarrantyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parts = await _context.Parts.ToListAsync();
            return Ok(parts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Part part)
        {
            part.Id = Guid.NewGuid();
            _context.Parts.Add(part);
            await _context.SaveChangesAsync();
            return Ok(part);
        }
    }
}
