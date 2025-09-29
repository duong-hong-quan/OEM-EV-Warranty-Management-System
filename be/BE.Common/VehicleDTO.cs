using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    public class VehicleDTO
    {
        public Guid? Id { get; set; }
        public string VIN { get; set; }
        public string VehicleName { get; set; }
        public Guid? CustomerId { get; set; }
    }

    public class VehicleWithDetailsDTO
    {
        public Guid Id { get; set; }
        public string VIN { get; set; }
        public string VehicleName { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerDTO? Customer { get; set; }
        public List<PartDTO>? Parts { get; set; }
        public List<ServiceHistoryDTO>? ServiceHistories { get; set; }
        public List<WarrantyClaimDTO>? WarrantyClaims { get; set; }
    }
}
