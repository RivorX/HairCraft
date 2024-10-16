using HAIRCRAFT.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User model)
    {
        if (ModelState.IsValid)
        {
            // Sprawdzenie, czy użytkownik już istnieje
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ViewBag.RegisterError = "Użytkownik z tym emailem już istnieje.";
                return View(model);
            }

            // Dodaj użytkownika do bazy
            _context.Users.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        // Jeśli walidacja nie powiodła się, wyświetl formularz ponownie
        return View(model);
    }


    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email))
        {
            ModelState.AddModelError("email", "Adres email jest wymagany.");
        }
        if (string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("password", "Hasło jest wymagane.");
        }

        if (ModelState.IsValid)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                // Utworzenie roszczenia
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Zalogowany użytkownik na stałe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index", "Home");
            }

            // Komunikat o błędzie, gdy dane logowania są błędne
            ViewBag.LoginError = "Nieprawidłowy email lub hasło.";
        }

        return View();
    }


    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
