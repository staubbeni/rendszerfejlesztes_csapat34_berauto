using BerAuto.Data;
using BerAuto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
            {
                return NotFound();
            }

            return rental;
        }

        [HttpPost]
        public async Task<ActionResult<Rental>> CreateRental(Rental rental)
        {
            var car = await _context.Cars.FindAsync(rental.CarId);
            if (car == null)
            {
                return BadRequest("Az autó nem található.");
            }

            if (!car.IsAvailable)
            {
                return BadRequest("Az autó jelenleg nem elérhető.");
            }

            var customer = await _context.Customers.FindAsync(rental.CustomerId);
            if (customer == null)
            {
                return BadRequest("Az ügyfél nem található.");
            }

            // Számoljuk ki a teljes költséget
            var days = (rental.ReturnDate - rental.RentalDate).Days;
            if (days <= 0)
            {
                return BadRequest("A visszahozatali dátumnak a bérlési dátum után kell lennie.");
            }

            rental.TotalCost = days * car.DailyRate;
            
            // Állítsuk az autót nem elérhetőre
            car.IsAvailable = false;
            
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, rental);
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }

            if (rental.IsCompleted)
            {
                return BadRequest("A bérlés már be van fejezve.");
            }

            var car = await _context.Cars.FindAsync(rental.CarId);
            if (car == null)
            {
                return BadRequest("Az autó nem található.");
            }

            rental.IsCompleted = true;
            car.IsAvailable = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 