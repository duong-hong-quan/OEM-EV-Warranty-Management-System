using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BE.DAL.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string VIN { get; set; }
        public string VehicleName { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        [JsonIgnore]
        public List<Part>? Parts { get; set; }
        [JsonIgnore]

        public List<ServiceHistory>? ServiceHistories { get; set; }
        [JsonIgnore]


        public List<WarrantyClaim>? WarrantyClaims { get; set; }
    }
}
