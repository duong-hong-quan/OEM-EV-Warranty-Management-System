using BE.Common;
using BE.DAL.Models;
using BE.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.API.Controllers
{
    [ApiController]
    [Route("api/customer")]
    [Authorize] // This will require authentication for all endpoints in this controller
    public class CustomersController : ControllerBase
    {
        private ICustomerService _service;
        public CustomersController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")] // Only Admin and Manager can view all customers
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string keyword)
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

        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admin can create customers
        public async Task<IActionResult> Create(CustomerDTO customer)
        {
            return await _service.Create(customer) is Customer createdCustomer ? Ok(createdCustomer) : BadRequest();
        }
    }
}
