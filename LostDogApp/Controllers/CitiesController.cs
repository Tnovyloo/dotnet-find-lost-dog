using Microsoft.AspNetCore.Mvc;
using LostDogApp.Models;
using LostDogApp.Data;
using LostDogApp.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LostDogApp.ViewModels.CityViewModels;

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
        public IActionResult Index(string? nameFilter, string? voivodeshipFilter)
        {
            var query = _context.Cities.AsQueryable();

            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(c => c.Name.Contains(nameFilter));
            }

            if (!string.IsNullOrEmpty(voivodeshipFilter))
            {
                query = query.Where(c => c.Voivodeship.Contains(voivodeshipFilter));
            }

            var cities = query
                .Select(c => new CityViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Voivodeship = c.Voivodeship,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude
                }).ToList();

            var viewModel = new CityIndexViewModel
            {
                NameFilter = nameFilter,
                VoivodeshipFilter = voivodeshipFilter,
                Cities = cities
            };

            return View(viewModel);
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


        // GET: Cities/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Cities/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityViewModel city)
        {
            if (ModelState.IsValid)
            {
                var cityModel = new City
                {
                    Id = city.Id,
                    Name = city.Name,
                    Voivodeship = city.Voivodeship,
                    Latitude = city.Latitude,
                    Longitude = city.Longitude
                };


                _context.Cities.Add(cityModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(city);
        }


        // GET: Cities/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var city = await _context.Cities.FindAsync(id);
            if (city == null) return NotFound();

            var cityViewModel = new CityViewModel
            {
                Id = city.Id,
                Name = city.Name,
                Voivodeship = city.Voivodeship,
                Latitude = city.Latitude,
                Longitude = city.Longitude
            };

            return View(cityViewModel); 
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CityViewModel city)
        {
            if (id != city.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var cityModel = new City
                {
                    Id = city.Id,
                    Name = city.Name,
                    Voivodeship = city.Voivodeship,
                    Latitude = city.Latitude,
                    Longitude = city.Longitude
                };


                try
                {
                    _context.Update(cityModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Cities.Any(c => c.Id == cityModel.Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(city);
        }

        // GET: Cities/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
            if (city == null) return NotFound();

                        var cityViewModel = new CityViewModel
            {
                Id = city.Id,
                Name = city.Name,
                Voivodeship = city.Voivodeship,
                Latitude = city.Latitude,
                Longitude = city.Longitude
            };

            return View(cityViewModel); 
        }

        // POST: Cities/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city != null)
            {
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
