namespace BerAuto.DataContext.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Odometer { get; set; }
        public bool IsAvailable { get; set; }

        public List<Rental> Rentals { get; set; }
        public int CategoryId { get; set; }
        public CarCategory Category { get; set; }
    }
} 