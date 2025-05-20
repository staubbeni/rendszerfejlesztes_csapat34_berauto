using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BerAuto.DataContext.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<RentalDto> Rentals { get; set; }
        public List<AddressDto> Address { get; set; }
        public List<string> Roles { get; set; } // Hozzáadva
    }

    public class UserRegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
        public AddressDto Address { get; set; }
    }

    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UserUpdateDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public IList<int> RoleIds { get; set; }
    }
}