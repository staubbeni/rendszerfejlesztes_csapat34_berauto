using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using BerAuto.Services;
using BerAuto.DataContext.Dtos;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        // Publikus: bérelhető autók listázása
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        // Publikus: részletes adat
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
                return NotFound();
            return Ok(car);
        }

        // Csak Admin: új autó
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CarCreateDto carCreateDto)
        {
            var created = await _carService.CreateCarAsync(carCreateDto);
            return CreatedAtAction(nameof(Details), new { id = created.Id }, created);
        }

        // Csak Admin: módosítás
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CarUpdateDto carUpdateDto)
        {
            var updated = await _carService.UpdateCarAsync(id, carUpdateDto);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        // Csak Admin: törlés
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _carService.DeleteCarAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }

        // Admin és Employee: elérhetőség állítása
        [HttpPut("{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> SetAvailability(int id, [FromQuery] bool available)
        {
            var result = await _carService.SetAvailabilityAsync(id, available);
            if (!result)
                return NotFound();
            return Ok();
        }

        // Csak Admin: kilométeróra állás módosítása
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOdometer(int id, [FromQuery] int newReading)
        {
            var result = await _carService.UpdateOdometerAsync(id, newReading);
            if (!result)
                return NotFound();
            return Ok();
        }

        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }
    }
}
