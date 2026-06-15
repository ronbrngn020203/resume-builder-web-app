using Microsoft.AspNetCore.Mvc;
using ResumeBuilderWebApp.Helpers;
using ResumeBuilderWebApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace ResumeBuilderWebApp.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart") ?? new List<CartItem>();
            ViewBag.Total = cart.Sum(c => c.Total);
            return View(cart);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.ServiceId == id);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObjectAsJson("cart", cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("cart");
            return RedirectToAction("Index");
        }
    }
}
