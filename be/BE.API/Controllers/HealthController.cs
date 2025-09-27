using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.API.Controllers
{
    [Route("api/health")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        /// <summary>
        /// Health check endpoint to verify if the service is running.
        /// </summary>
        /// <returns>Returns "Healthy" if the service is running.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHealth()
        {
            return Ok();
        }
    }
}
