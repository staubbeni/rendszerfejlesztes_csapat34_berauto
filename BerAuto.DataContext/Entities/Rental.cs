using BerAuto.DataContext.Entities;

namespace BerAuto.DataContext.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalCost { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
} 