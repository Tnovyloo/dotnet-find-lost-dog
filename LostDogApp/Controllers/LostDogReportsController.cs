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

namespace LostDogApp.Controllers
{
    public class LostDogReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; // Add UserManager here

        // Inject UserManager<ApplicationUser> into the constructor
        public LostDogReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; // Initialize _userManager
        }

        // GET: LostDogReports
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserId = _userManager.GetUserId(User);
            }

            var reports = await _context.LostDogReports.ToListAsync();
            return View(reports);
        }


        // GET: LostDogReports/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostDogReport = await _context.LostDogReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lostDogReport == null)
            {
                return NotFound();
            }

            return View(lostDogReport);
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
        public async Task<IActionResult> Create(LostDogReport model)
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
                model.UserId = userId;
                Console.WriteLine($"Creating report for user: {model.UserId}"); // Add logging
                
                _context.Add(model);
                try
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Report created successfully"); // Add logging
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating report: {ex.Message}"); // Add logging
                    ModelState.AddModelError("", "An error occurred while saving the report.");
                }
            }
            return View(model);
        }

        // GET: LostDogReports/Edit/5
        [Authorize]
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

            return View(report);
        }

        // POST: LostDogReports/Edit/5
        // POST: LostDogReports/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LostDogReport model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var report = await _context.LostDogReports.FindAsync(id);
            if (report == null || report.UserId != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    report.DogName = model.DogName;
                    report.Description = model.Description;
                    report.Latitude = model.Latitude;
                    report.Longitude = model.Longitude;
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LostDogReportExists(model.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
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
