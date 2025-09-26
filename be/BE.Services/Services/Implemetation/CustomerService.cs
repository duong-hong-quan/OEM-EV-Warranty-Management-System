using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.DAL.DTO;
using BE.DAL.GenericRepository;
using BE.DAL.Models;
using BE.DAL.UOW;
using Microsoft.EntityFrameworkCore;

namespace BE.Services.Services.Implemetation
{
    public class CustomerService : BaseService, ICustomerService
    {
        private IGenericRepository<Customer> _customerRepo;
        private IUnitOfWork _unitOfWork;
        public CustomerService(IServiceProvider serviceProvider, IGenericRepository<Customer> customerRepo, IUnitOfWork unitOfWork) : base(serviceProvider)
        {
            _customerRepo = customerRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Customer>> Get(QueryOptions<Customer> queryOptions)
        {
            return await _customerRepo.GetAllDataByExpression(new QueryOptions<Customer>
            {
                Filter = null,
                PageNumber = queryOptions.PageNumber,
                PageSize = queryOptions.PageSize,
                OrderBy = queryOptions.OrderBy
            });
        }
        public async Task<Customer> Create(CustomerDTO customer)
        {
            Customer customerEntity = new Customer
            {
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address
            };
            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                var vehicleRepo = Resolve<IGenericRepository<Vehicle>>();

                customer.Id = Guid.NewGuid();
                await _customerRepo.Insert(customerEntity);
                if (customer.Vehicles != null && customer.Vehicles.Count > 0)
                {
                    foreach (var vehicle in customer.Vehicles)
                    {
                        Vehicle vehicleEntity = new Vehicle
                        {
                            VIN = vehicle.VIN,
                            VehicleName = vehicle.VehicleName,
                            CustomerId = customerEntity.Id
                        };
                        vehicle.Id = Guid.NewGuid();
                        await vehicleRepo.Insert(vehicleEntity);

                    }
                }
                await _unitOfWork.SaveChangesAsync();
            });

            return customerEntity;
        }
    }
}
