using System.ComponentModel.DataAnnotations;

namespace LostDogApp.ViewModels.LostDogReportViewModels
{

    public class UpdateViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Imie psa jest wymagane.")]
        public string DogName { get; set; }
        
        public string Description { get; set; }

        [Required(ErrorMessage = "Numer kontaktowy jest wymagany.")]
        public required string ContactNumber { get; set; }
        
        [Required(ErrorMessage = "Szerokość geograficzna jest wymagana.")]
        public double Latitude { get; set; }
        
        [Required(ErrorMessage = "Długość geograficzna jest wymagana.")]
        public double Longitude { get; set; }
        
        [Display(Name = "Dog Photo")]
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
        public int? CityId { get; set; }
    }
}