using System.ComponentModel.DataAnnotations;

namespace LostDogApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Nazwa uzytkownika jest wymagana")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Haslo jest wymagane")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
