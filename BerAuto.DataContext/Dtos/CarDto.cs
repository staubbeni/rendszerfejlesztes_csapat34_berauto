using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerAuto.DataContext.Dtos
{
    public class CarDto
    {
        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        public int Year { get; set; }
        public int Odometer { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class CarCreateDto
    {
        [Required]
        public string Make { get; set; }

        public string Model { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int CarCategoryId { get; set; }
    }

    public class CarUpdateDto
    {
        [Required]
        public string Make { get; set; }

        public string Model { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        public int CarCategoryId { get; set; }
    }
}
