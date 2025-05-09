namespace LostDogApp.ViewModels.CityViewModels
{
    public class CityIndexViewModel
    {
        public string? NameFilter { get; set; }
        public string? VoivodeshipFilter { get; set; }

        public List<CityViewModel> Cities { get; set; } = new();
    }
}
