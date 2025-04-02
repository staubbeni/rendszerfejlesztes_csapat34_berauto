using Microsoft.AspNetCore.Mvc;
using BerAuto.Services;
using BerAuto.DataContext.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BerAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableCars()
        {
            var cars = await _carService.GetAvailableCarsAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")] 
        public async Task<IActionResult> AddCar([FromBody] Car car)
        {
            var createdCar = await _carService.AddCarAsync(car);
            return CreatedAtAction(nameof(GetCar), new { id = createdCar.Id }, createdCar);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] Car car)
        {
            if (id != car.Id) return BadRequest();
            await _carService.UpdateCarAsync(car);
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _carService.DeleteCarAsync(id);
            return NoContent();
        }
    }
}
