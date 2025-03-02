using System;

namespace BerAuto.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public int? RentalRequestId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public int StartMileage { get; set; }
        public int? EndMileage { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsReturned { get; set; }
        public string Status { get; set; } // Active, Completed, Cancelled

        // Navigációs tulajdonságok
        public User User { get; set; }
        public Car Car { get; set; }
        public RentalRequest RentalRequest { get; set; }
        public Invoice Invoice { get; set; }
    }
} 