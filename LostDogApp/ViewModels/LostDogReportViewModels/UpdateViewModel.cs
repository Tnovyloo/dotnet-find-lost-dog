using System.ComponentModel.DataAnnotations;

namespace LostDogApp.ViewModels.LostDogReportViewModels
{

    public class UpdateViewModel
    {
        public int Id { get; set; }
        
        [Required]
        public string DogName { get; set; }
        
        public string Description { get; set; }

        [Required]
        public required string ContactNumber { get; set; }
        
        [Required]
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }
        
        [Display(Name = "Dog Photo")]
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
        public int? CityId { get; set; }
    }
}