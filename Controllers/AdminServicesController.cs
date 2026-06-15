using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResumeBuilderWebApp.Models;

namespace ResumeBuilderWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminServices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Services.Include(s => s.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AdminServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: AdminServices/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.ServiceCategories, "CategoryId", "Name");
            return View();
        }

        // POST: AdminServices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)
            {
                // 👇 this shows what’s wrong
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        Console.WriteLine($"Field: {kvp.Key} - Error: {error.ErrorMessage}");
                    }
                }

                // repopulate dropdown when ModelState fails
                ViewData["CategoryId"] = new SelectList(_context.ServiceCategories, "CategoryId", "Name", service.CategoryId);
                return View(service);
            }

            // ✅ ModelState valid → save
            _context.Add(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.ServiceCategories, "CategoryId", "Name", service.CategoryId);
            return View(service);
        }

        // POST: AdminServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,CategoryId,Title,ShortDescription,FullDescription,Price,DurationMinutes,ThumbnailPath,IsActive,CreatedAt")] Service service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
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
            ViewData["CategoryId"] = new SelectList(_context.ServiceCategories, "CategoryId", "Name", service.CategoryId);
            return View(service);
        }

        // GET: AdminServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: AdminServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}
