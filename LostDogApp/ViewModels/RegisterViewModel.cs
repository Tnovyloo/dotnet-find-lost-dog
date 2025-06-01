using System.ComponentModel.DataAnnotations;

namespace LostDogApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nazwa uzytkownika jest wymagana")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Hasło musi byc takie samo")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Hasło musi byc takie samo")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
