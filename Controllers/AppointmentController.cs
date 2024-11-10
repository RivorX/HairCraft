using HAIRCRAFT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HAIRCRAFT.Controllers
{

    public class AppointmentController : Controller
    {
        private readonly UserManager<User> _userManager; 
        private readonly ApplicationDbContext _context; 

        public AppointmentController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize] // Upewnij się, że użytkownik jest zalogowany
        public async Task<IActionResult> MyAppointments()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Pobierz wizyty zalogowanego użytkownika wraz z powiązanym salonem
            var appointments = await _context.Appointments
                .Where(a => a.ClientId == userId)
                .Include(a => a.Salon)
                .ToListAsync();

            return View(appointments);
        }

        // Akcja do odwoływania wizyty
        [HttpPost]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound(); // Jeśli wizyta nie istnieje, zwróć 404
            }

            // Sprawdzamy, czy wizyta może być odwołana (np. nie jest już zakończona)
            if (appointment.AppointmentDate <= DateTime.Now)
            {
                return BadRequest("Nie możesz odwołać wizyty, która już się odbyła.");
            }

            // Usuwamy wizytę
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            // Po udanym usunięciu przekierowujemy do strony z wizytami
            return RedirectToAction(nameof(MyAppointments));
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

            // Sprawdzenie, czy dana godzina jest dostępna
            var existingAppointment = await _context.Appointments
                .Where(a => a.SalonId == appointment.SalonId &&
                            a.AppointmentDate < appointment.AppointmentDate.AddMinutes(30) && // Zajęta godzina
                            a.AppointmentDate.AddMinutes(30) > appointment.AppointmentDate)  // Zajęta godzina
                .FirstOrDefaultAsync();

            if (existingAppointment != null)
            {
                ModelState.AddModelError("", "Wybrana godzina jest już zajęta. Proszę wybrać inną.");

                // Ustawienie dostępnych usług w przypadku błędu
                var salon = await _context.Salons.Include(s => s.Services).FirstOrDefaultAsync(s => s.Id == appointment.SalonId);
                if (salon == null)
                {
                    return NotFound(); // Jeśli salon nie istnieje, zwróć błąd
                }

                ViewBag.Services = salon.Services; // Przekazanie usług do widoku
                ViewBag.SalonId = appointment.SalonId; // Przekazanie salonId do widoku

                return View(salon); // Zwróć widok z modelem salonu
            }

            // Ustawienie identyfikatora klienta jako zalogowanego użytkownika
            appointment.ClientId = userId;

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



        // Akcja do oceniania wizyty
        [HttpPost]
        public async Task<IActionResult> RateAppointment([FromBody] RateAppointmentModel model)
        {
            var appointment = await _context.Appointments.FindAsync(model.AppointmentId);

            if (appointment == null || appointment.AppointmentDate >= DateTime.Now)
            {
                return BadRequest("Nie można ocenić tej wizyty.");
            }

            // Dodaj ocenę do wizyty (wymaga odpowiedniego pola w modelu Appointment)
            appointment.Rating = model.Rating;
            await _context.SaveChangesAsync();

            return Ok("Ocena została zapisana.");
        }



    }
}
