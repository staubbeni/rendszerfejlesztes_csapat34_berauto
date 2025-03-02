using System;

namespace BerAuto.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        
        // Navigációs tulajdonság
        public virtual Rental Rental { get; set; }
    }
} 