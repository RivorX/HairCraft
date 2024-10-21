using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HAIRCRAFT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class SalonController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    public SalonController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize(Roles = "Fryzjer")]
    public IActionResult Index()
    {
        var ownerId = _userManager.GetUserId(User); // Używanie IdentityUser
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
            var ownerId = _userManager.GetUserId(User); // Pobieranie aktualnego użytkownika
            salon.OwnerId = ownerId;

            _context.Salons.Add(salon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(salon);
    }





    // Strona do edytowania istniejącego salonu
    [HttpGet]
    [Authorize(Roles = "Fryzjer")]
    public async Task<IActionResult> Edit(int id)
    {
        var salon = await _context.Salons.FindAsync(id);
        if (salon == null)
        {
            return NotFound();
        }

        // Upewnij się, że użytkownik jest właścicielem salonu
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (salon.OwnerId.ToString() != ownerId)
        {
            return Forbid(); // Zwraca 403 Forbidden
        }

        return View(salon);
    }

    // Obsługa edytowania salonu
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

    // Strona do usuwania salonu
    [HttpPost]
    [Authorize(Roles = "Fryzjer")]
    public async Task<IActionResult> Delete(int id)
    {
        var salon = await _context.Salons.FindAsync(id);
        if (salon == null)
        {
            return NotFound();
        }

        // Upewnij się, że użytkownik jest właścicielem salonu
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (salon.OwnerId.ToString() != ownerId)
        {
            return Forbid(); // Zwraca 403 Forbidden
        }

        _context.Salons.Remove(salon);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
