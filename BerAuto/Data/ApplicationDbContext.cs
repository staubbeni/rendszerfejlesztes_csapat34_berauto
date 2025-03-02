using BerAuto.Models;
using Microsoft.EntityFrameworkCore;

namespace BerAuto.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Decimal típusok pontosságának beállítása
            modelBuilder.Entity<Car>()
                .Property(c => c.DailyRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Rental>()
                .Property(r => r.TotalCost)
                .HasPrecision(18, 2);

            // Kapcsolatok beállítása
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CarId);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CustomerId);

            // Kezdeti adatok
            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2020, LicensePlate = "ABC-123", DailyRate = 15000, IsAvailable = true },
                new Car { Id = 2, Make = "Honda", Model = "Civic", Year = 2021, LicensePlate = "DEF-456", DailyRate = 17000, IsAvailable = true },
                new Car { Id = 3, Make = "Ford", Model = "Focus", Year = 2019, LicensePlate = "GHI-789", DailyRate = 13000, IsAvailable = true }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Kovács János", Email = "kovacs.janos@example.com", Phone = "06301234567", Address = "Budapest, Példa utca 1.", DriverLicense = "AB123456" },
                new Customer { Id = 2, Name = "Nagy Éva", Email = "nagy.eva@example.com", Phone = "06209876543", Address = "Debrecen, Minta utca 2.", DriverLicense = "CD789012" }
            );
        }
    }
} 