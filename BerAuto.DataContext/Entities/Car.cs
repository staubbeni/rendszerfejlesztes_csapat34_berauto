namespace BerAuto.DataContext.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } // A DTO-ban 'Make'-k�nt szerepel
        public string Model { get; set; }
        public int CategoryId { get; set; }
        public CarCategory Category { get; set; }
        public decimal DailyRate { get; set; } // Kor�bban hozz�adva
        public bool IsAvailable { get; set; } // Hozz�adva
        public int Odometer { get; set; } // Hozz�adva
    }
}