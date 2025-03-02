namespace BerAuto.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
} 