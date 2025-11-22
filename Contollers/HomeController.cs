using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gayrimenkul.Data;
using Gayrimenkul.Models;

namespace Gayrimenkul.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Home/Index - Tüm ilanları listele
        public async Task<IActionResult> Index(string searchCity, int? categoryId)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SearchCity = searchCity;

            var properties = _context.Properties
                .Include(p => p.Category)
                .Include(p => p.User)
                .Where(p => p.IsActive);

            // Kategori filtresi
            if (categoryId.HasValue && categoryId > 0)
            {
                properties = properties.Where(p => p.CategoryId == categoryId);
            }

            // Şehir filtresi
            if (!string.IsNullOrEmpty(searchCity))
            {
                properties = properties.Where(p => p.City.Contains(searchCity));
            }

            return View(await properties.OrderByDescending(p => p.CreatedAt).ToListAsync());
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            // Kullanıcı girişi kontrolü
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "İlan eklemek için giriş yapmalısınız!";
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Price,City,District,Address,SquareMeters,Rooms,Bathrooms,Floor,ImageUrl,CategoryId")] Property property)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                property.UserId = userId.Value;
                property.CreatedAt = DateTime.Now;
                property.IsActive = true;

                _context.Add(property);
                await _context.SaveChangesAsync();

                TempData["Success"] = "İlan başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(property);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            // Sadece ilan sahibi düzenleyebilir
            if (property.UserId != userId)
            {
                TempData["Error"] = "Bu ilanı düzenleme yetkiniz yok!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(property);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,City,District,Address,SquareMeters,Rooms,Bathrooms,Floor,ImageUrl,CategoryId,UserId,CreatedAt,IsActive")] Property property)
        {
            if (id != property.Id)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || property.UserId != userId)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(property);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "İlan başarıyla güncellendi!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(property.Id))
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

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(property);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var property = await _context.Properties
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            if (property.UserId != userId)
            {
                TempData["Error"] = "Bu ilanı silme yetkiniz yok!";
                return RedirectToAction(nameof(Index));
            }

            return View(property);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var property = await _context.Properties.FindAsync(id);

            if (property != null && property.UserId == userId)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
                TempData["Success"] = "İlan başarıyla silindi!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }
    }
}