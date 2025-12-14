using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gayrimenkul.Data;
using Gayrimenkul.Models;

namespace Gayrimenkul.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FullName,Email,Password,Phone")] User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Bu email adresi zaten kullanılıyor!");
                    return View(user);
                }

                user.CreatedAt = DateTime.Now;
                _context.Add(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email ve şifre zorunludur!";
                return View();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Email veya şifre hatalı!";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);

            TempData["Success"] = $"Hoş geldiniz, {user.FullName}!";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = await _context.Users.FindAsync(userId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(User model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != model.Id) return RedirectToAction("Login");

            ModelState.Remove("Email"); 
            ModelState.Remove("Password"); 

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.FullName = model.FullName;
                user.Phone = model.Phone;
                
                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.Password = model.Password;
                }

                _context.Update(user);
                await _context.SaveChangesAsync();
                
                HttpContext.Session.SetString("UserName", user.FullName);
                TempData["Success"] = "Profil bilgileri güncellendi.";
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Çıkış yapıldı!";
            return RedirectToAction("Index", "Home");
        }
    }
}