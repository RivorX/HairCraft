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

    // Wy�wietlanie listy salon�w
    public IActionResult Index()
    {
        var salons = _context.Salons.ToList(); // Pobranie salon�w z bazy danych
        return View(salons); // Przekazanie salon�w do widoku
    }

    // Widok rejestracji u�ytkownik�w
    [AllowAnonymous]
    public IActionResult Register()
    {
        return RedirectToAction("Register", "Account"); // Przekierowanie do AccountController
    }

    // Widok logowania u�ytkownik�w
    [AllowAnonymous]
    public IActionResult Login()
    {
        return RedirectToAction("Login", "Account"); // Przekierowanie do AccountController
    }

    // Wylogowanie u�ytkownika
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme); // Wylogowanie u�ytkownika
        return RedirectToAction("Index");
    }
}
