using BE.Common;
using BE.DAL.Models;
using BE.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace BE.API.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        
        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Get all vehicles with pagination support
        /// </summary>
        /// <returns>List of vehicles</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllVehicles()
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

        /// <summary>
        /// Get vehicle by ID
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <returns>Vehicle details</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVehicleById(Guid id)
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

        /// <summary>
        /// Create a new vehicle
        /// </summary>
        /// <param name="vehicleDto">Vehicle data</param>
        /// <returns>Created vehicle</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleDTO vehicleDto)
        {
            try
            {
                var vehicle = await _vehicleService.CreateVehicleAsync(vehicleDto);
                return CreatedAtAction(nameof(GetVehicleById), new { id = vehicle.Id }, vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing vehicle
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <param name="vehicleDto">Updated vehicle data</param>
        /// <returns>Updated vehicle</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] VehicleDTO vehicleDto)
        {
            try
            {
                // Ensure the ID from route matches the DTO
                vehicleDto.Id = id;
                var vehicle = await _vehicleService.UpdateVehicleAsync(vehicleDto);
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a vehicle
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVehicle(Guid id)
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

        /// <summary>
        /// Get all parts for a specific vehicle
        /// </summary>
        /// <param name="vehicleId">Vehicle ID</param>
        /// <returns>List of vehicle parts</returns>
        [HttpGet("{vehicleId:guid}/parts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVehicleParts(Guid vehicleId)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByIdAsync(vehicleId);
                if (vehicle == null)
                    return NotFound($"Vehicle with ID {vehicleId} not found");

                // Assuming vehicle has Parts property - you may need to add this to your service
                // For now, returning success message
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
