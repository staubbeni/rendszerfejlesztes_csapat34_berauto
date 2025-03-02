using BerAuto.DataContext.Entities;

namespace BerAuto.DataContext.Entities
{
    public class CarCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Car> Cars { get; set; }
    }
}