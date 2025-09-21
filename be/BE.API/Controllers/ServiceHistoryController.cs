using BE.Common;
using BE.DAL.Models;
using BE.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace BE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceHistoryController : ControllerBase
    {
        private readonly IServiceHistoryService _serviceHistoryService;
        
        public ServiceHistoryController(IServiceHistoryService serviceHistoryService)
        {
            _serviceHistoryService = serviceHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var histories = await _serviceHistoryService.GetAllServiceHistoriesAsync();
                return Ok(histories);
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
                var history = await _serviceHistoryService.GetServiceHistoryByIdAsync(id);
                if (history == null)
                    return NotFound();
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceHistoryDTO historyDto)
        {
            try
            {
                var history = await _serviceHistoryService.CreateServiceHistoryAsync(historyDto);
                return CreatedAtAction(nameof(GetById), new { id = history.Id }, history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(ServiceHistoryDTO historyDto)
        {
            try
            {
                var history = await _serviceHistoryService.UpdateServiceHistoryAsync(historyDto);
                return Ok(history);
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
                var success = await _serviceHistoryService.DeleteServiceHistoryAsync(id);
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
                var result = await _serviceHistoryService.GetByVehicleIdAsync(vehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
