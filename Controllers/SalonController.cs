using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HAIRCRAFT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HAIRCRAFT.Controllers
{
    public class SalonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public SalonController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Fryzjer")]
        public IActionResult Index()
        {
            var ownerId = _userManager.GetUserId(User);
            var salons = _context.Salons.Where(s => s.OwnerId == ownerId).ToList();
            return View(salons);
        }

        [HttpGet]
        [Authorize(Roles = "Fryzjer")]
        public IActionResult Create()
        {
            // Pobieranie OwnerId z aktualnie zalogowanego użytkownika
            var ownerId = _userManager.GetUserId(User);
            ViewBag.OwnerId = ownerId; // Możesz użyć ViewBag do przekazania OwnerId do widoku
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Fryzjer")]
        public async Task<IActionResult> Create(Salon salon)
        {
            if (ModelState.IsValid)
            {
                // Przypisanie OwnerId do salonu
                salon.OwnerId = salon.OwnerId ?? _userManager.GetUserId(User);

                // Debugging: logowanie salon
                Console.WriteLine($"Salon przed dodaniem: Name = {salon.Name}, OwnerId = {salon.OwnerId}");

                _context.Salons.Add(salon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Jeśli ModelState nie jest ważny, logujemy błędy
            Console.WriteLine("Błędy walidacji: ");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(salon);
        }

        [HttpGet]
        [Authorize(Roles = "Fryzjer")]
        public async Task<IActionResult> Edit(int id)
        {
            var salon = await _context.Salons.FindAsync(id);
            if (salon == null)
            {
                return NotFound();
            }

            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (salon.OwnerId != ownerId)
            {
                return Forbid();
            }

            return View(salon);
        }

        [HttpPost]
        [Authorize(Roles = "Fryzjer")]
        public async Task<IActionResult> Edit(Salon salon)
        {
            if (ModelState.IsValid)
            {
                _context.Update(salon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(salon);
        }

        [HttpPost]
        [Authorize(Roles = "Fryzjer")]
        public async Task<IActionResult> Delete(int id)
        {
            var salon = await _context.Salons.FindAsync(id);
            if (salon == null)
            {
                return NotFound();
            }

            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (salon.OwnerId != ownerId)
            {
                return Forbid();
            }

            _context.Salons.Remove(salon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
