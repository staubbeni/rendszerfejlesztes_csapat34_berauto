using System.ComponentModel.DataAnnotations;

namespace BerAuto.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "A felhasználónév megadása kötelező!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        public string Password { get; set; }
    }
} 