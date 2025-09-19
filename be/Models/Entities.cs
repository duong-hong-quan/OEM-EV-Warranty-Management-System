using System;
using System.Collections.Generic;

namespace ElectricVehicleWarranty.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }

    public class Vehicle
    {
        public Guid Id { get; set; }
        public string VIN { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<Part> Parts { get; set; }
        public List<ServiceHistory> ServiceHistories { get; set; }
        public List<WarrantyClaim> WarrantyClaims { get; set; }
    }

    public class Part
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }

    public class ServiceHistory
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Description { get; set; }
        public string Technician { get; set; }
    }

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
