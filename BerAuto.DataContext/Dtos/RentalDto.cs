using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BerAuto.DataContext.Entities;

namespace BerAuto.DataContext.Dtos
{
    public class RentalDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime StartDate { get; set; }
        public int TotalCost { get; set; }
        public string Status { get; set; }
    }

    public class OrderCreateDto
    {
        [Required]
        public IEnumerable<RentalCarCreateDto> Items { get; set; }
        public int AddressId { get; set; }
        public int? UserId { get; set; }
        public int RentalId { get; set; }
    }

    public class RentalCarCreateDto 
    {
        [Required]
        public int RentalCarId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}