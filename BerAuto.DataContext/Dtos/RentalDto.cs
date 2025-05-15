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
        [Required]
        public int CarId { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        [Required]
        public string GuestName { get; set; }
        [Required]
        [EmailAddress]
        public string GuestEmail { get; set; }
        [Required]
        [Phone]
        public string GuestPhone { get; set; }
        [Required(ErrorMessage = "Guest address is required")]
        public string GuestAddress { get; set; }
    }
}
