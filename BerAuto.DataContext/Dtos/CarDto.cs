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
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public CarCategoryDto Category { get; set; }

    }
    public class CarCreateDto
    {
        [Required]

        public string Name { get; set; }

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
        public string Name { get; set; }
        public string Model { get; set; }
        public decimal? Price { get; set; }
        public int CarCategoryId { get; set; }
    }

}
