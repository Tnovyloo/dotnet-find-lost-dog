using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LostDogApp.Data;
using LostDogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LostDogApp.ViewModels.LostDogReportViewModels;
using LostDogApp.Models;
using LostDogApp.Utils;

namespace LostDogApp.Controllers
{
    public class LostDogReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Inject UserManager<ApplicationUser> into the constructor
        public LostDogReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; // Initialize _userManager
        }

        // GET: LostDogReports
        [AllowAnonymous]
        public async Task<IActionResult> Index(string selectedVoivodeship, string selectedCity, string dogNameQuery)
        {
            var cities = await _context.Cities.ToListAsync();
            var reportsQuery = _context.LostDogReports
                .Include(r => r.City)
                .AsQueryable();

            if (!string.IsNullOrEmpty(selectedVoivodeship))
                reportsQuery = reportsQuery.Where(r => r.City != null && r.City.Voivodeship == selectedVoivodeship);

            if (!string.IsNullOrEmpty(selectedCity))
                reportsQuery = reportsQuery.Where(r => r.City != null && r.City.Name == selectedCity);

            if (!string.IsNullOrEmpty(dogNameQuery))
                reportsQuery = reportsQuery.Where(r => r.DogName.Contains(dogNameQuery));

            var model = new IndexFilterViewModel
            {
                SelectedVoivodeship = selectedVoivodeship,
                SelectedCity = selectedCity,
                DogNameQuery = dogNameQuery,
                AvailableVoivodeships = cities.Select(c => c.Voivodeship).Distinct().OrderBy(v => v).ToList(),
                AvailableCities = cities.Select(c => c.Name).Distinct().OrderBy(n => n).ToList(),
                Reports = await reportsQuery.ToListAsync()
            };

            return View(model);
        }

        // GET: LostDogReports/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Include both User and Comments→User
            var report = await _context.LostDogReports
                .Include(r => r.User)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null) return NotFound();

            var vm = new DetailsViewModel
            {
                Id            = report.Id,
                DogName       = report.DogName,
                Description   = report.Description,
                ImagePath     = report.ImagePath,
                Latitude      = report.Latitude,
                Longitude     = report.Longitude,
                ContactNumber = report.ContactNumber,
                UserName      = report.User?.UserName,
                Comments      = report.Comments
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList()
            };

            return View(vm);
        }


        // GET: LostDogReports/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LostDogReports/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReportViewModel model)
        {

            string? userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId)) {
                return Challenge();
            }

            if (model.Latitude == null || model.Longitude == null)
            {
                ModelState.AddModelError("", "Both latitude and longitude are required");
            }

            ModelState.Remove("User");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {   
                if (model.ImageFile.Length > 10 * 1024 * 1024) // 10MB limit
                {
                    ModelState.AddModelError("ImageFile", "File size cannot exceed 10MB");
                    return View(model);
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only image files (JPG, PNG, GIF) are allowed.");
                    return View(model);
                }

                string imagePath = null;
                string fileName = null;
                string contentType = null;

                if (model.ImageFile != null && model.ImageFile.Length > 0) 
                {
                    // Create unique name
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    contentType = model.ImageFile.ContentType;

                    // Set path to save (in wwwroot/images)
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "dogs");
                    Directory.CreateDirectory(uploadsFolder);

                    // Create the full file path
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    // Store relative path for web access
                    imagePath = $"/images/dogs/{fileName}";
                    model.ImagePath = imagePath;
                }

                var allCities = await _context.Cities.ToListAsync();
                var nearestCity = allCities
                    .Select(c => new
                    {
                        City = c,
                        Distance = GeoUtils.CalculateHaversineDistance(
                            model.Latitude, model.Longitude,
                            c.Latitude, c.Longitude)
                    })
                    .OrderBy(x => x.Distance)
                    .FirstOrDefault();

                LostDogReport report = new LostDogReport
                {
                    DogName = model.DogName,
                    Description = model.Description,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    UserId = userId,
                    ImagePath = imagePath,
                    ImageFileName = fileName,
                    ImageContentType = contentType,
                    ContactNumber = model.ContactNumber,
                    CityId = nearestCity?.City.Id,
                };

                _context.Add(report);
                try
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Report created successfully");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating report: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the report.");
                }
            }
            return View(model);
        }

        // GET: LostDogReports/Edit/5
        [Authorize]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.LostDogReports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (report.UserId != userId)
            {
                return Forbid();
            }

            // Convert to ViewModel
            var viewModel = new UpdateViewModel
            {
                Id = report.Id,
                DogName = report.DogName,
                Description = report.Description,
                Latitude = report.Latitude,
                Longitude = report.Longitude,
                ImagePath = report.ImagePath,
                ContactNumber = report.ContactNumber,
            };

            return View(viewModel);
        }

        // POST: LostDogReports/Edit/5
        // TODO Make it to change City and Voivodeship while updating
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateViewModel model, bool removeImage = false)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var existingReport = await _context.LostDogReports.FindAsync(id);
            if (existingReport == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (existingReport.UserId != currentUserId)
            {
                return Forbid();
            }

            if (model.ImageFile == null)
            {
                ModelState.Remove(nameof(model.ImageFile));
            }

            // Handle image removal
            if (removeImage && !string.IsNullOrEmpty(existingReport.ImagePath))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingReport.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                existingReport.ImagePath = null;
                existingReport.ImageFileName = null;
                existingReport.ImageContentType = null;
            }

            // Handle new image upload
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Delete old image if it exists
                if (!string.IsNullOrEmpty(existingReport.ImagePath))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingReport.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Process new image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "dogs");
                Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                existingReport.ImagePath = $"/images/dogs/{fileName}";
                existingReport.ImageFileName = fileName;
                existingReport.ImageContentType = model.ImageFile.ContentType;
            }

            // Update other fields
            existingReport.DogName = model.DogName;
            existingReport.Description = model.Description;
            existingReport.Latitude = model.Latitude;
            existingReport.Longitude = model.Longitude;
            existingReport.ContactNumber = model.ContactNumber;

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(existingReport);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LostDogReportExists(model.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            // If we got here, something went wrong
            model.ImagePath = existingReport.ImagePath; // Preserve image path
            return View(model);
        }

        // GET: LostDogReports/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.LostDogReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (report.UserId != userId)
            {
                return Forbid();
            }

            return View(report);
        }

        // POST: LostDogReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.LostDogReports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (report.UserId != userId)
            {
                return Forbid();
            }

            _context.LostDogReports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool LostDogReportExists(int id)
        {
            return _context.LostDogReports.Any(e => e.Id == id);
        }
    }
}
