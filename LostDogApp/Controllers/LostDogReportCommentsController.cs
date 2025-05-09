using System;
using System.Linq;
using System.Threading.Tasks;
using LostDogApp.Data;
using LostDogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostDogApp.Controllers
{
    public class LostDogReportCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LostDogReportCommentsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /LostDogReportComments/Create?reportId=123
        [Authorize]
        public IActionResult Create(int reportId)
        {
            // Verify the report exists
            if (!_context.LostDogReports.Any(r => r.Id == reportId))
                return NotFound();

            // Pass the reportId into the view
            var vm = new LostDogReportComment { LostDogReportId = reportId };
            return View(vm);
        }

        // POST: LostDogReportComments/Create
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Create(LostDogReportComment comment)
        {
            if (!_context.LostDogReports.Any(r => r.Id == comment.LostDogReportId))
                ModelState.AddModelError("", "Report not found.");

            ModelState.Remove(nameof(comment.User));
            ModelState.Remove(nameof(comment.UserId));
            ModelState.Remove(nameof(comment.LostDogReport));

            if (!ModelState.IsValid)
                return View(comment);

            comment.UserId    = _userManager.GetUserId(User);
            comment.CreatedAt = DateTime.UtcNow;

            _context.LostDogReportComments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details", 
                "LostDogReports", 
                new { id = comment.LostDogReportId }
            );
        }

        // GET: /LostDogReportComments/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.LostDogReportComments
                .Include(c => c.LostDogReport)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null) return NotFound();
            if (comment.UserId != _userManager.GetUserId(User))
                return Forbid();

            return View(comment);
        }

        // POST: /LostDogReportComments/Delete/5
        [HttpPost, ActionName("Delete"), Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.LostDogReportComments.FindAsync(id);
            if (comment == null) return NotFound();
            if (comment.UserId != _userManager.GetUserId(User))
                return Forbid();

            _context.LostDogReportComments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { reportId = comment.LostDogReportId });
        }
    }
}
