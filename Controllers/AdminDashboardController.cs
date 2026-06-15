using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeBuilderWebApp.Models;

namespace ResumeBuilderWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get simple statistics
            var totalUsers = _context.Users.Count();
            var totalGraduates = _context.Users.Count(u => u.Role == "Graduate");

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalGraduates = totalGraduates;

            return View();
        }
    }
}
