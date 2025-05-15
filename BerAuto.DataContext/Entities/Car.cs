namespace BerAuto.DataContext.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } // A DTO-ban 'Make'-ként szerepel
        public string Model { get; set; }
        public int CategoryId { get; set; }
        public CarCategory Category { get; set; }
        public decimal DailyRate { get; set; } // Korábban hozzáadva
        public bool IsAvailable { get; set; } // Hozzáadva
        public int Odometer { get; set; } // Hozzáadva
    }
}