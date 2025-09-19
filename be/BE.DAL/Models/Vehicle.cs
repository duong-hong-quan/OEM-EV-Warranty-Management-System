using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string VIN { get; set; }
        public string VehicleName { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<Part> Parts { get; set; }
        public List<ServiceHistory> ServiceHistories { get; set; }
        public List<WarrantyClaim> WarrantyClaims { get; set; }
    }
}
