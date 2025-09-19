using Microsoft.EntityFrameworkCore;

namespace BE.DAL.Models
{
    public class WarrantyDbContext : DbContext, IDbContext
    {
        public WarrantyDbContext(DbContextOptions<WarrantyDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ServiceHistory> ServiceHistories { get; set; }
        public DbSet<WarrantyClaim> WarrantyClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
