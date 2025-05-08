using Microsoft.AspNetCore.Mvc;
using LostDogApp.Models;
using LostDogApp.Data;
using LostDogApp.Seeders;
using Microsoft.AspNetCore.Authorization;

namespace LostDogApp.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public CitiesController(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        // GET: Cities/Index
        [HttpGet]
        public IActionResult Index()
        {
            var cities = _context.Cities.ToList();
            return View(cities);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SeedCities()
        {
            try
            {
                // Call the Seeder
                await CitySeeder.SeedCitiesAsync(_serviceProvider);

                System.Console.WriteLine("City seeding successful.");
                return RedirectToAction("Index", "Cities");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred while seeding cities: {ex}");
                return View("Error");
            }
        }
    }
}
