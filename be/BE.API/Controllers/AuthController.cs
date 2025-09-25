using BE.Common;
using BE.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(request.RefreshToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var success = await _authService.RevokeTokenAsync(request.RefreshToken);
                if (success)
                    return Ok(new { message = "Token revoked successfully" });
                else
                    return BadRequest(new { message = "Token not found" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                var user = await _authService.GetUserByIdAsync(userId);
                
                if (user == null)
                    return NotFound(new { message = "User not found" });
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Demo endpoints for 401 and 403 responses
        [HttpGet("demo/unauthorized")]
        public IActionResult DemoUnauthorized()
        {
            return Unauthorized(new { message = "401 - You are not authenticated. Please login first." });
        }

        [HttpGet("demo/forbidden")]
        [Authorize(Roles = "Admin")]
        public IActionResult DemoForbidden()
        {
            return Ok(new { message = "This endpoint requires Admin role" });
        }

        [HttpGet("demo/customer-only")]
        [Authorize(Roles = "Customer")]
        public IActionResult DemoCustomerOnly()
        {
            return Ok(new { message = "This endpoint is accessible to Customers only" });
        }

        [HttpGet("demo/manager-only")]
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult DemoManagerOnly()
        {
            return Ok(new { message = "This endpoint requires Manager or Admin role" });
        }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }
}
