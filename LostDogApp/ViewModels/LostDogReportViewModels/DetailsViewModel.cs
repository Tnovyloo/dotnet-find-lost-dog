namespace LostDogApp.ViewModels.LostDogReportViewModels {
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public string DogName { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ContactNumber { get; set; }
        public string? UserName { get; set; }
    }
}
