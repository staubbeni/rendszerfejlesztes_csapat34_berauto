using Microsoft.AspNetCore.Mvc;
using BerAuto.Service;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalController(IRentalService RentalService)
        {
            _rentalService = RentalService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _carService.GetAllRental();
            return Ok(result);
        }
    }
}