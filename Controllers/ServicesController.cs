using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeBuilderWebApp.Helpers;
using ResumeBuilderWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeBuilderWebApp.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ServicesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Services
        // Shows all categories
        public async Task<IActionResult> Index()
        {
            var categories = await _db.ServiceCategories
                              .OrderBy(c => c.Name)
                              .ToListAsync();

            return View(categories);
        }

        // GET: /Services/Category/{slug}
        // Shows all services under a specific category
        public async Task<IActionResult> Category(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return NotFound();

            var category = await _db.ServiceCategories
                .Include(c => c.Services.Where(s => s.IsActive))
                .FirstOrDefaultAsync(c => c.Slug == slug);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // GET: /Services/Details/{id}
        // Shows full details of a selected service
        public async Task<IActionResult> Details(int id)
        {
            var service = await _db.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.ServiceId == id && s.IsActive);

            if (service == null)
                return NotFound();

            ViewData["PageType"] = "Service";
            return View(service);
        }

        // POST: /Services/AddToCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int id)
        {
            var service = _db.Services.FirstOrDefault(s => s.ServiceId == id);
            if (service == null) return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.ServiceId == id);
            if (existingItem == null)
            {
                cart.Add(new CartItem
                {
                    ServiceId = service.ServiceId,
                    Title = service.Title,
                    Price = service.Price,
                    Quantity = 1
                });
            }
            else
            {
                existingItem.Quantity++;
            }

            HttpContext.Session.SetObjectAsJson("cart", cart);
            TempData["Message"] = $"{service.Title} added to your cart.";

            return RedirectToAction("Details", new { id });
        }
    }
}