using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResumeBuilderWebApp.Helpers;
using ResumeBuilderWebApp.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ResumeBuilderWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        // Matches the old unsalted-SHA256-hex format this app used to store passwords in.
        private static readonly Regex LegacySha256HexPattern = new("^[a-f0-9]{64}$", RegexOptions.Compiled);

        public AccountController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            PasswordVerificationResult verification;

            if (LegacySha256HexPattern.IsMatch(user.Password ?? string.Empty))
            {
                // One-time migration path for accounts still holding the old SHA-256 hex hash.
                var matchesLegacyHash = user.Password == LegacySha256Hex(password);
                verification = matchesLegacyHash
                    ? PasswordVerificationResult.SuccessRehashNeeded
                    : PasswordVerificationResult.Failed;
            }
            else if (IsPasswordHasherFormat(user.Password))
            {
                // A real hash produced by IPasswordHasher<T> — verify normally.
                verification = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            }
            else
            {
                // Anything else — leftover plaintext, a manually-typed temp value, or any other
                // string that isn't recognizably one of our two known hash formats. Comparing
                // directly avoids feeding non-hash data into VerifyHashedPassword, which can
                // decode "successfully" as garbage bytes and silently report Failed instead of
                // throwing, even when the value was never meant to be a hash at all.
                verification = user.Password == password
                    ? PasswordVerificationResult.SuccessRehashNeeded
                    : PasswordVerificationResult.Failed;
            }

            if (verification == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            // Transparently re-hash into the new format (covers both legacy migration and an outdated work factor)
            if (verification == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.Password = _passwordHasher.HashPassword(user, password);
                await _context.SaveChangesAsync();
            }

            var claims = new List<Claim>
    {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return user.Role switch
            {
                "Admin" => RedirectToAction("Index", "AdminDashboard"),
                "Graduate" => RedirectToAction("Index", "GraduateDashboard"),
                _ => RedirectToAction("Login", "Account")
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private static string LegacySha256Hex(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password ?? string.Empty));
            var builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }

        // ASP.NET Core's PasswordHasher<T> prefixes its output with a version marker byte:
        // 0x00 = legacy V2 format (49 bytes total), 0x01 = current V3 format (13+ byte header).
        // Anything that doesn't match this shape — even if it happens to be valid Base64,
        // like a short manually-typed value — isn't a real hash, so we treat it as plaintext instead.
        private static bool IsPasswordHasherFormat(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            byte[] decoded;
            try
            {
                decoded = Convert.FromBase64String(value);
            }
            catch (FormatException)
            {
                return false;
            }

            if (decoded.Length == 0)
                return false;

            return (decoded[0] == 0x00 && decoded.Length == 49)
                || (decoded[0] == 0x01 && decoded.Length > 13);
        }
    }
}