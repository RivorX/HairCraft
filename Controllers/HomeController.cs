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

    // Wyœwietlanie listy salonów
    public IActionResult Index()
    {
        var salons = _context.Salons.Include(s => s.Services).ToList();
        return View(salons);
    }


    // Widok rejestracji u¿ytkowników
    [AllowAnonymous]
    public IActionResult Register()
    {
        return RedirectToAction("Register", "Account"); // Przekierowanie do AccountController
    }

    // Widok logowania u¿ytkowników
    [AllowAnonymous]
    public IActionResult Login()
    {
        return RedirectToAction("Login", "Account"); // Przekierowanie do AccountController
    }

    // Wylogowanie u¿ytkownika
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme); // Wylogowanie u¿ytkownika
        return RedirectToAction("Index");
    }
}
