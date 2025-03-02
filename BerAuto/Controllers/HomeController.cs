using BerAuto.Data;
using BerAuto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BerAuto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var availableCars = await _context.Cars.Where(c => c.IsAvailable).ToListAsync();
            return View(availableCars);
        }

        public async Task<IActionResult> Rentals()
        {
            var rentals = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .ToListAsync();
            return View(rentals);
        }

        public async Task<IActionResult> CarDetails(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            
            if (car == null)
            {
                return NotFound();
            }
            
            return View(car);
        }
    }
} 