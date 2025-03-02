using BerAuto.Models;
using Microsoft.EntityFrameworkCore;

namespace BerAuto.Data
{
    public class BerAutoContext : DbContext
    {
        public BerAutoContext(DbContextOptions<BerAutoContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<RentalRequest> RentalRequests { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Táblák nevének beállítása, hogy megfeleljen a meglévő adatbázisnak
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Car>().ToTable("Cars");
            modelBuilder.Entity<RentalRequest>().ToTable("RentalRequests");
            modelBuilder.Entity<Rental>().ToTable("Rentals");
            modelBuilder.Entity<Invoice>().ToTable("Invoices");
            
            // Felhasználó - Kölcsönzési kérelem kapcsolat
            modelBuilder.Entity<RentalRequest>()
                .HasOne(r => r.User)
                .WithMany(u => u.RentalRequests)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Autó - Kölcsönzési kérelem kapcsolat
            modelBuilder.Entity<RentalRequest>()
                .HasOne(r => r.Car)
                .WithMany(c => c.RentalRequests)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Felhasználó - Kölcsönzés kapcsolat
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Autó - Kölcsönzés kapcsolat
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Kölcsönzési kérelem - Kölcsönzés kapcsolat
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.RentalRequest)
                .WithOne(rr => rr.Rental)
                .HasForeignKey<Rental>(r => r.RentalRequestId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Kölcsönzés - Számla kapcsolat
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Rental)
                .WithOne(r => r.Invoice)
                .HasForeignKey<Invoice>(i => i.RentalId)
                .OnDelete(DeleteBehavior.Cascade);

            // User konfigurálása
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Car konfigurálása
            modelBuilder.Entity<Car>()
                .HasIndex(c => c.LicensePlate)
                .IsUnique();
        }
    }
} 