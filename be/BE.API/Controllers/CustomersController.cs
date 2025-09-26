using BE.Common;
using BE.DAL.Models;
using BE.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [Authorize] // This will require authentication for all endpoints in this controller
    public class CustomersController : ControllerBase
    {
        private ICustomerService _service;
        public CustomersController(ICustomerService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all customers with pagination and search
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <param name="keyword">Search keyword</param>
        /// <returns>Paginated list of customers</returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")] // Only Admin and Manager can view all customers
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllCustomers(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string keyword = "")
        {
            var customers = await _service.Get(new DAL.DTO.QueryOptions<Customer>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Filter = s => s.Name.Contains(keyword) || s.Phone.Contains(keyword),
                OrderBy = new List<(string PropertyName, bool Ascending)>
                {
                    (nameof(Customer.Name), true)
                },
            });
            return Ok(customers);
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="customer">Customer data</param>
        /// <returns>Created customer</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admin can create customers
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customer)
        {
            return await _service.Create(customer) is Customer createdCustomer ? Ok(createdCustomer) : BadRequest();
        }

       
    }
}
