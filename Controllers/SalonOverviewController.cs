using HAIRCRAFT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HAIRCRAFT.Controllers
{
    [Authorize]
    public class SalonOverviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalonOverviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Akcja dla widoku "Pokaż Salon"
        public async Task<IActionResult> SalonOverview(int id)
        {
            var salon = await _context.Salons.Include(s => s.Services).FirstOrDefaultAsync(s => s.Id == id);
            if (salon == null)
            {
                return NotFound();
            }
            return View("SalonOverview", salon);
        }
    }
}
