using Microsoft.EntityFrameworkCore;
using ElectricVehicleWarranty.Models;

namespace ElectricVehicleWarranty.Data
{
    public class WarrantyDbContext : DbContext
    {
        public WarrantyDbContext(DbContextOptions<WarrantyDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ServiceHistory> ServiceHistories { get; set; }
        public DbSet<WarrantyClaim> WarrantyClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ...existing code...
        }
    }
}
