using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BerAuto.DataContext.Entities;

namespace BerAuto.DataContext.Dtos
{
    public class RentalRequestDto
    {
        [Required]
        public int CarId { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        // Vendég adatainak megadása, ha nincs bejelentkezve
        public string GuestName { get; set; }
        [EmailAddress]
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public string GuestAddress { get; set; }
    }

    public class RentalDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public string GuestAddress { get; set; }

        public int CarId { get; set; }
        public string CarMakeModel { get; set; }

        public DateTime RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public RentalStatus Status { get; set; }
        public decimal TotalCost { get; set; }
    }
}
