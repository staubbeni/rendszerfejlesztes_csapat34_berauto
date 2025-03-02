using System;
using System.Collections.Generic;

namespace BerAuto.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Mileage { get; set; }
        public bool IsAvailable { get; set; }
        public decimal DailyRate { get; set; }
        public string Category { get; set; }
        public string FuelType { get; set; }
        public int Seats { get; set; }
        public string Transmission { get; set; }
        public string Description { get; set; }
        
        // Navigációs tulajdonságok
        public ICollection<Rental> Rentals { get; set; }
        public virtual ICollection<RentalRequest> RentalRequests { get; set; }
    }
} 