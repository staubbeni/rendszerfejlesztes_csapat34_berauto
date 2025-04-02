using Microsoft.AspNetCore.Mvc;
using BerAuto.Services;
using BerAuto.DataContext.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BerAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateRental([FromBody] Rental rental)
        {
            var createdRental = await _rentalService.CreateRentalAsync(rental);
            return CreatedAtAction(nameof(GetRental), new { id = createdRental.Id }, createdRental);
        }

        [HttpGet("user/{userId}")]
        //[Authorize]
        public async Task<IActionResult> GetUserRentals(int userId)
        {
            var rentals = await _rentalService.GetUserRentalsAsync(userId);
            return Ok(rentals);
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> GetRental(int id)
        {
            var rental = await _rentalService.GetRentalByIdAsync(id);
            if (rental == null) return NotFound();
            return Ok(rental);
        }

        [HttpPut("{id}/status")]
        //[Authorize(Roles = "Agent")] 
        public async Task<IActionResult> UpdateRentalStatus(int id, [FromBody] string status)
        {
            await _rentalService.UpdateRentalStatusAsync(id, status);
            return NoContent();
        }
    }
}