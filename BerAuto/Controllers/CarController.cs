using Microsoft.AspNetCore.Mvc;
using BerAuto.Services;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService CarService)
        {
            _carService = CarService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _carService.GetAllCars();
            return Ok(result);
        }
    }
}
