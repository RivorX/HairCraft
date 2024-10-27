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
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Fryzjer")]
        public async Task<IActionResult> Create(Salon salon)
        {
            if (ModelState.IsValid)
            {
                salon.OwnerId = salon.OwnerId ?? _userManager.GetUserId(User);
                _context.Salons.Add(salon);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
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
                catch (Exception ex)
                {
                    // Debug: Log exception to console for further inspection
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "Wystąpił problem przy edytowaniu salonu. Proszę spróbować ponownie.");
                }
            }
            else
            {
                // Debug: Log model state errors
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return RedirectToAction("Index", "Home");
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

            // Przekierowanie do Index w kontrolerze Home
            return RedirectToAction("Index", "Home");
        }


    }
}
