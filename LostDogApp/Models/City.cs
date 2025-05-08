using CsvHelper.Configuration.Attributes;

namespace LostDogApp.Models {
    public class City
    {
        public int Id { get; set; }

        [Name("Miasto")]
        public string Name { get; set; }

        [Name("Wojewodztwo")]
        public string Voivodeship { get; set; }

        [Name("Latitude")]
        public double Latitude { get; set; }

        [Name("Longitude")]
        public double Longitude { get; set; }

        // Navigation property (1 City → many DogReports)
        public ICollection<LostDogReport> LostDogReports { get; set; }
    }

}