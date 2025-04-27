using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BerAuto.Services;

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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult List()
        {
            var result = _carService.GetAllCars();
            return Ok(result);
        }
    }
}
