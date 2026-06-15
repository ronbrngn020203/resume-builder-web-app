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
    [Authorize(Roles = "Graduate")]
    public class ResumesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<ResumesController> _logger;

        public ResumesController(ApplicationDbContext context, ILogger<ResumesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Resumes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Resumes.ToListAsync());
        }

        // GET: Resumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .FirstOrDefaultAsync(m => m.ResumeId == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Resume
            {
                Educations = new List<Education> { new Education() },
                Experiences = new List<Experience> { new Experience() },
                Skills = new List<Skill> { new Skill() }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Resume resume)
        {
            Console.WriteLine("?? Received Create request");

            if (resume.Educations != null)
                Console.WriteLine($"Educations count: {resume.Educations.Count}");
            if (resume.Experiences != null)
                Console.WriteLine($"Experiences count: {resume.Experiences.Count}");
            if (resume.Skills != null)
                Console.WriteLine($"Skills count: {resume.Skills.Count}");

            if (ModelState.IsValid)
            {
                // Ensures child entities are linked to parent before saving
                foreach (var edu in resume.Educations)
                    edu.Resume = resume;
                foreach (var exp in resume.Experiences)
                    exp.Resume = resume;
                foreach (var skill in resume.Skills)
                    skill.Resume = resume;

                resume.CreatedAt = DateTime.Now;

                _context.Resumes.Add(resume);
                await _context.SaveChangesAsync();

                Console.WriteLine("? Resume saved successfully!");
                return RedirectToAction("Preview", new { id = resume.ResumeId });
            }

            Console.WriteLine("? ModelState invalid.");
            foreach (var error in ModelState)
            {
                foreach (var subError in error.Value.Errors)
                {
                    Console.WriteLine($"? ModelState Error: {error.Key} - {subError.ErrorMessage}");
                }
            }

            // Ensures lists exist when returning invalid model
            resume.Educations ??= new List<Education> { new Education() };
            resume.Experiences ??= new List<Experience> { new Experience() };
            resume.Skills ??= new List<Skill> { new Skill() };

            return View(resume);
        }

        // GET: Resumes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .Include(r => r.Educations)
                .Include(r => r.Experiences)
                .Include(r => r.Skills)
                .FirstOrDefaultAsync(r => r.ResumeId == id);

            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // POST: Resumes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Resume resume)
        {
            if (id != resume.ResumeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resume);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Resumes.Any(e => e.ResumeId == resume.ResumeId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Preview), new { id = resume.ResumeId });
            }
            return View(resume);
        }

        public async Task<IActionResult> Preview(int id)
        {
            var resume = await _context.Resumes
                .Include(r => r.Educations)
                .Include(r => r.Experiences)
                .Include(r => r.Skills)
                .FirstOrDefaultAsync(r => r.ResumeId == id);

            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // GET: Resumes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .FirstOrDefaultAsync(m => m.ResumeId == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // POST: Resumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume != null)
            {
                _context.Resumes.Remove(resume);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResumeExists(int id)
        {
            return _context.Resumes.Any(e => e.ResumeId == id);
        }
    }
}
