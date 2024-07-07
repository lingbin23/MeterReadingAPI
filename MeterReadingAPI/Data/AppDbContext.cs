using Microsoft.EntityFrameworkCore;
using MeterReadingAPI.Models;

namespace MeterReadingAPI.Data
{
    // Data/AppDbContext.cs
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure primary key
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.AccountId);

            modelBuilder.Entity<MeterReading>()
               .HasKey(c => c.Id);
        }

    }
}
