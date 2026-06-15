using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeBuilderWebApp.Models;
using System.Linq;
using System.Security.Claims;

namespace ResumeBuilderWebApp.Controllers
{
    [Authorize(Roles = "Graduate")]
    public class GraduateDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GraduateDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get the logged-in user's email
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            // Find their resumes
            var resumes = _context.Resumes
                .Where(r => r.Email == userEmail)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            ViewBag.UserEmail = userEmail;
            ViewBag.ResumeCount = resumes.Count;

            return View(resumes);
        }
    }
}
