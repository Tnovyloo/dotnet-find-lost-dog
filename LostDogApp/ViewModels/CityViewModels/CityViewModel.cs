using LostDogApp.Models;

namespace LostDogApp.ViewModels.CityViewModels {
    public class CityViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Voivodeship { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}