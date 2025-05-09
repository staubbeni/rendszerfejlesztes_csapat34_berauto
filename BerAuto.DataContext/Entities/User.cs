using BerAuto.DataContext.Entities;

namespace BerAuto.DataContext.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<Address> Address { get; set; } = new List<Address>(); // Inicializálás
        public List<Rental> Rentals { get; set; }
        public List<Role> Roles { get; set; }
    }

}
