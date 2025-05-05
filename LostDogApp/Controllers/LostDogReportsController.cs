using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LostDogApp.Data;
using LostDogApp.Models;

namespace LostDogApp.Controllers
{
    public class LostDogReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LostDogReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LostDogReports
        public async Task<IActionResult> Index()
        {
            return View(await _context.LostDogReports.ToListAsync());
        }

        // GET: LostDogReports/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: LostDogReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DogName,Description,Latitude,Longitude,UserId")] LostDogReport lostDogReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lostDogReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lostDogReport);
        }

        // GET: LostDogReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostDogReport = await _context.LostDogReports.FindAsync(id);
            if (lostDogReport == null)
            {
                return NotFound();
            }
            return View(lostDogReport);
        }

        // POST: LostDogReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DogName,Description,Latitude,Longitude,UserId")] LostDogReport lostDogReport)
        {
            if (id != lostDogReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lostDogReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LostDogReportExists(lostDogReport.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lostDogReport);
        }

        // GET: LostDogReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: LostDogReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lostDogReport = await _context.LostDogReports.FindAsync(id);
            if (lostDogReport != null)
            {
                _context.LostDogReports.Remove(lostDogReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LostDogReportExists(int id)
        {
            return _context.LostDogReports.Any(e => e.Id == id);
        }
    }
}
