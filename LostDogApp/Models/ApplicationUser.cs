using LostDogApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LostDogApp.Models {
    public class ApplicationUser : IdentityUser
    {
        public ICollection<LostDogReport> LostDogReports { get; set; }

    }
}

