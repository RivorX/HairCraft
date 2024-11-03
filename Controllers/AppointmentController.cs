using HAIRCRAFT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HAIRCRAFT.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly UserManager<User> _userManager; // Użyj swojej niestandardowej klasy User
        private readonly ApplicationDbContext _context; // Zakładam, że masz ApplicationDbContext

        public AppointmentController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Akcja wyświetlania formularza rezerwacji wizyty
        public IActionResult Book(int salonId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Ustawienie salonu i usług
            var salon = _context.Salons.Include(s => s.Services).FirstOrDefault(s => s.Id == salonId);
            if (salon == null)
            {
                return NotFound();
            }

            // Ustawiamy ViewBag.Services, aby były dostępne w widoku
            ViewBag.Services = salon.Services;

            ViewBag.SalonId = salonId; // Przekazanie salonId do widoku
            return View(salon); // Przekazanie modelu salonu do widoku
        }


        // Akcja obsługująca zapis rezerwacji
        [HttpPost]
        public async Task<IActionResult> Book(Appointment appointment)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Ustawienie identyfikatora klienta jako zalogowanego użytkownika
            appointment.ClientId = userId;

            // Sprawdzenie, czy dane są poprawne
            if (appointment == null)
            {
                ModelState.AddModelError("", "Nieprawidłowe dane rezerwacji.");
                return View(appointment); // Zwróć widok z błędami
            }

            // Dodanie rezerwacji do bazy danych
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { appointmentId = appointment.Id });
        }


        public async Task<IActionResult> Confirmation(int appointmentId)
        {
            // Znajdowanie rezerwacji na podstawie identyfikatora
            var appointment = await _context.Appointments
                .Include(a => a.Client)  // Dołącz klienta
                .Include(a => a.Salon)   // Dołącz salon
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            // Przekazanie modelu do widoku
            return View(appointment);
        }

    }
}
