using LostDogApp.Models;

namespace LostDogApp.Models
{
    public class LostDogReport
    {
        public int Id { get; set; }
        public string DogName { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string ContactNumber { get; set; }

        // Image properties
        public string ImagePath { get; set; }  // Stores the file path
        public string ImageFileName { get; set; }  // Original file name
        public string ImageContentType { get; set; }  // MIME type
            
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }

        public ICollection<LostDogReportComment> Comments { get; set; }

    }
}
