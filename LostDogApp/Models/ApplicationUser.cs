using LostDogApp.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public ICollection<LostDogReport> LostDogReports { get; set; }
}
