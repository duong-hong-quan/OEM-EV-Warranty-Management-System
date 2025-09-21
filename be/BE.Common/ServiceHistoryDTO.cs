using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    public class ServiceHistoryDTO
    {
        public Guid? Id { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime ServiceDate { get; set; }
        public string? Description { get; set; }
        public string? Technician { get; set; }
    }
}
