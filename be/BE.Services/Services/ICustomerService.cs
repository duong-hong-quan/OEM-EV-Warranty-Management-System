using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.DAL.DTO;
using BE.DAL.Models;

namespace BE.Services.Services
{
    public interface ICustomerService
    {
        Task<PagedResult<Customer>> Get(QueryOptions<Customer> queryOptions);
        Task<Customer> Create(CustomerDTO customer);

    }
}
