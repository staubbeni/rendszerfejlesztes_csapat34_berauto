namespace CarRentalApp.DataContext.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public int DailyRate { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public CarCategory Category { get; set; }
    }
} 