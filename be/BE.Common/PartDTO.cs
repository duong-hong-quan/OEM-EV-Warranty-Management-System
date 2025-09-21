using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    public class PartDTO
    {
        public Guid? Id { get; set; }
        public string? SerialNumber { get; set; }
        public string? Name { get; set; }
        public Guid VehicleId { get; set; }
    }
}
