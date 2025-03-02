using System;

namespace BerAuto.Models
{
    public class RentalRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } // "Pending", "Approved", "Rejected"
        public string Notes { get; set; }
        
        // Navigációs tulajdonságok
        public virtual User User { get; set; }
        public virtual Car Car { get; set; }
        public virtual Rental Rental { get; set; }
    }
} 