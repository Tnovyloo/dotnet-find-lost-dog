using System.ComponentModel.DataAnnotations;

namespace LostDogApp.ViewModels.LostDogReportViewModels
{

    public class ReportViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Imię psa jest wymagane.")]
        public string DogName { get; set; }
        
        [Required(ErrorMessage = "Opis jest wymagany.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Numer kontaktowy jest wymagany.")]
        public required string ContactNumber { get; set; }
        
        [Required(ErrorMessage = "Szerokość geograficzna jest wymagana.")]
        public double Latitude { get; set; }
        
        [Required(ErrorMessage = "Długość geograficzna jest wymagana.")]
        public double Longitude { get; set; }

        [Display(Name = "Dog Photo")]
        [Required(ErrorMessage = "Zdjęcie jest wymagane")]
        public IFormFile ImageFile { get; set; }  // For file upload
        
        public string? ImagePath { get; set; } // To store and show existing path.
    }
}