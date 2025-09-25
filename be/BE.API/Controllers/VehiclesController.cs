using BE.Common;
using BE.DAL.Models;
using BE.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace BE.API.Controllers
{
    [ApiController]
    [Route("api/vehicle")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        
        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                return Ok(vehicles);
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
                var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
                if (vehicle == null)
                    return NotFound();
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleDTO vehicleDto)
        {
            try
            {
                var vehicle = await _vehicleService.CreateVehicleAsync(vehicleDto);
                return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(VehicleDTO vehicleDto)
        {
            try
            {
                var vehicle = await _vehicleService.UpdateVehicleAsync(vehicleDto);
                return Ok(vehicle);
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
                var success = await _vehicleService.DeleteVehicleAsync(id);
                if (!success)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{vehicleId}/parts")]
        public async Task<IActionResult> AddPart(Guid vehicleId, PartDTO partDto)
        {
            try
            {
                var part = await _vehicleService.AddPartIntoVehicle(vehicleId, partDto);
                return Ok(part);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("parts")]
        public async Task<IActionResult> UpdatePart(PartDTO partDto)
        {
            try
            {
                var part = await _vehicleService.EditPartVehicle(partDto);
                return Ok(part);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("parts/{partId}")]
        public async Task<IActionResult> RemovePart(Guid partId)
        {
            try
            {
                var part = await _vehicleService.RemovePartVehicle(partId);
                return Ok(part);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
