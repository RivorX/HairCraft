using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Wyświetlanie listy salonów
    public IActionResult Index()
    {
        var salons = _context.Salons.Include(s => s.Services).ToList();
        return View(salons);
    }


    // Widok rejestracji użytkowników
    [AllowAnonymous]
    public IActionResult Register()
    {
        return RedirectToAction("Register", "Account"); // Przekierowanie do AccountController
    }

    // Widok logowania użytkowników
    [AllowAnonymous]
    public IActionResult Login()
    {
        return RedirectToAction("Login", "Account"); // Przekierowanie do AccountController
    }

    // Wylogowanie użytkownika
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme); // Wylogowanie użytkownika
        return RedirectToAction("Index");
    }
}
