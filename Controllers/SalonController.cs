using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HAIRCRAFT.Models;
using Microsoft.EntityFrameworkCore;

public class SalonController : Controller
{
    private readonly ApplicationDbContext _context;

    public SalonController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Wyświetlenie listy salonów dla fryzjera
    [Authorize(Roles = "Fryzjer")]
    public IActionResult Index()
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Pobierz ID użytkownika
        var salons = _context.Salons.Where(s => s.OwnerId.ToString() == ownerId).ToList(); // Porównaj OwnerId jako string
        return View(salons);
    }

    // Strona do tworzenia nowego salonu
    [HttpGet]
    [Authorize(Roles = "Fryzjer")]
    public IActionResult Create()
    {
        return View();
    }

    // Obsługa tworzenia nowego salonu
    [HttpPost]
    [Authorize(Roles = "Fryzjer")]
    public async Task<IActionResult> Create(Salon salon)
    {
        // Pobierz OwnerId z tożsamości użytkownika
        var ownerIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (ModelState.IsValid)
        {
            // Sprawdzenie, czy fryzjer już ma salon
            var existingSalon = await _context.Salons.FirstOrDefaultAsync(s => s.OwnerId.ToString() == ownerIdString);

            if (existingSalon != null)
            {
                ModelState.AddModelError("", "Możesz posiadać tylko jeden salon.");
                return View(salon);
            }

            // Ustawienie OwnerId przed dodaniem salonu
            if (int.TryParse(ownerIdString, out int ownerId))
            {
                salon.OwnerId = ownerId; // Ustawienie OwnerId jako liczby całkowitej
            }
            else
            {
                ModelState.AddModelError("", "Niepoprawne ID właściciela: " + ownerIdString);
                return View(salon);
            }

            _context.Salons.Add(salon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // W przypadku błędów, zwróć widok z istniejącym modelem
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
