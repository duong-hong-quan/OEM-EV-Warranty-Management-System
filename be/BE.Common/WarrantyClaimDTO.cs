using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    public class WarrantyClaimDTO
    {
        public Guid? Id { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime? ClaimDate { get; set; }
        public string? Status { get; set; }
        public string? Report { get; set; }
        public string? DiagnosticInfo { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}
