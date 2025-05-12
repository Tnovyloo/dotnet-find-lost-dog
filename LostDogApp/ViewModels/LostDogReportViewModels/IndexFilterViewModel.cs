using LostDogApp.Models;

namespace LostDogApp.ViewModels.LostDogReportViewModels
{
    public class IndexFilterViewModel
    {
        public string SelectedVoivodeship { get; set; }
        public string SelectedCity { get; set; }
        public string DogNameQuery { get; set; }

        public List<string> AvailableVoivodeships { get; set; }
        public List<string> AvailableCities { get; set; }

        public IEnumerable<LostDogReport> Reports { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
