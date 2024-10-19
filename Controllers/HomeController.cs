using Microsoft.AspNetCore.Authentication;
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

    public IActionResult Index()
    {
        var salons = _context.Salons.ToList(); // Pobierz salony jako listê
        return View(salons); // Przeka¿ listê do widoku
    }


    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Logout()
    {
        // Zaloguj u¿ytkownika
        HttpContext.SignOutAsync();
        return RedirectToAction("Index");
    }
}
