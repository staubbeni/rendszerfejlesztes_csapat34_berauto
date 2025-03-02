using System.ComponentModel.DataAnnotations;

namespace BerAuto.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "A felhasználónév megadása kötelező!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        [StringLength(100, ErrorMessage = "A jelszónak legalább {2} karakter hosszúnak kell lennie.", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "A jelszavak nem egyeznek!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "A teljes név megadása kötelező!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező!")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail cím!")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
} 