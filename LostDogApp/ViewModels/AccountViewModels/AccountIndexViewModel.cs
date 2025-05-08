using LostDogApp.Models;

namespace LostDogApp.ViewModels.AccountViewModels {
    public class AccountIndexViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<LostDogReport> Reports { get; set; }
    }
}