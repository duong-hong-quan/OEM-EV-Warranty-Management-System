using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.Models
{
    public class WarrantyClaim
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime ClaimDate { get; set; }
        public string Status { get; set; } // Sent, Pending, Accepted, Processed
        public string Report { get; set; }
        public string DiagnosticInfo { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
