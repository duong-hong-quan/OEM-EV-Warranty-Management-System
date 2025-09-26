using BE.Common;
using BE.DAL.Models;
using BE.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.API.Controllers
{
    [ApiController]
    [Route("api/parts")]
    [Authorize] // Require authentication for all endpoints
    public class PartsController : ControllerBase
    {
        private readonly IPartService _partService;
        
        public PartsController(IPartService partService)
        {
            _partService = partService;
        }

        /// <summary>
        /// Get all parts
        /// </summary>
        /// <returns>List of all parts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllParts()
        {
            try
            {
                var parts = await _partService.GetAllPartsAsync();
                return Ok(parts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get part by ID
        /// </summary>
        /// <param name="id">Part ID</param>
        /// <returns>Part details</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPartById(Guid id)
        {
            try
            {
                var part = await _partService.GetPartByIdAsync(id);
                if (part == null)
                    return NotFound();
                return Ok(part);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Technician")] // Only staff can create parts
        public async Task<Part> Create(PartDTO partDto)
        {
            return await _partService.CreatePartAsync(partDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(PartDTO partDto)
        {
            try
            {
                var part = await _partService.UpdatePartAsync(partDto);
                return Ok(part);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can delete parts
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _partService.DeletePartAsync(id);
                if (!success)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
