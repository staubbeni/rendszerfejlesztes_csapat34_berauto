using System;
using System.Collections.Generic;

namespace BerAuto.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; } // Admin, Assistant, User
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

        // Navigációs tulajdonságok
        public ICollection<Rental> Rentals { get; set; }
        public ICollection<RentalRequest> RentalRequests { get; set; }
    }
} 