using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HAIRCRAFT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

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
        

        // Akcja dla widoku "Mój Salon"
        public async Task<IActionResult> MySalonDetails(int id)
        {
            var salon = await _context.Salons
                .Include(s => s.Services) // Załaduj usługi powiązane z salonem
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salon == null)
            {
                return NotFound();
            }

            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (salon.OwnerId != ownerId)
            {
                return Forbid();
            }

            return View("Details", salon);
        }
        

        [HttpGet]
        [Authorize(Roles = "Fryzjer")]
        public IActionResult Create()
        {
            ViewBag.UserId = _userManager.GetUserId(User);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Fryzjer")]
        public async Task<IActionResult> Create(Salon salon)
        {
            // Ustawienie OwnerId z UserManagera (jeśli jest puste)
            if (string.IsNullOrEmpty(salon.OwnerId))
            {
                salon.OwnerId = _userManager.GetUserId(User);
            }

            // Sprawdzenie, czy OwnerId nie jest pusty
            if (string.IsNullOrEmpty(salon.OwnerId))
            {
                ModelState.AddModelError("OwnerId", "OwnerId jest wymagane.");
            }

            // Walidacja modelu
            if (ModelState.IsValid)
            {
                _context.Salons.Add(salon);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Jeśli walidacja się nie powiodła, zwróć formularz wraz z błędami
                ModelState.AddModelError("", "Nie udało się dodać salonu. Proszę sprawdzić wszystkie pola.");
                return View(salon);
            }
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
                try
                {
                    _context.Update(salon);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Salons.Any(s => s.Id == salon.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
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

            return RedirectToAction("Index", "Home");
        }





        //-------------------------------------------------------------------------------- Usługi --------------------------------------------------------------------------------
        // Formularz dodawania nowej usługi
        [HttpGet]
        [Authorize(Roles = "Fryzjer")]
        public IActionResult AddService(int salonId)
        {
            ViewBag.SalonId = salonId;
            return View();
        }

        // Przetwarzanie dodawania nowej usługi
        [HttpPost]
        [Authorize(Roles = "Fryzjer")]
        public async Task<IActionResult> AddService(Service service)
        {
            // Sprawdzanie, czy SalonId jest przekazywane
            if (service.SalonId <= 0)
            {
                ModelState.AddModelError("SalonId", "SalonId jest wymagane.");
            }
            else
            {
                // Sprawdzenie, czy salon istnieje w bazie
                var salon = await _context.Salons.FindAsync(service.SalonId);
                if (salon == null)
                {
                    ModelState.AddModelError("SalonId", "Salon, do którego przypisujesz usługę, nie istnieje.");
                }
            }

            // Walidacja modelu
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = service.SalonId });
            }

            // Ustaw SalonId w ViewBag, aby zwrócić do widoku
            ViewBag.SalonId = service.SalonId;
            return View(service);
        }

        // Edytowanie usługi
        // Wyświetlenie formularza edycji usługi
        [HttpGet]
        [Authorize(Roles = "Fryzjer")]
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // Przetwarzanie edycji usługi
        [HttpPost]
        [Authorize(Roles = "Fryzjer")]
        public async Task<IActionResult> EditService(Service service)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = service.SalonId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Services.Any(s => s.Id == service.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(service);
        }


        // Usuwanie usługi
        [HttpPost]
        [Authorize(Roles = "Fryzjer")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", new { id = service.SalonId });
        }
    }
}
