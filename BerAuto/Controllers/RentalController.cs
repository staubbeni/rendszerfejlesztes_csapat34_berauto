using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BerAuto.Services;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Employee,Admin")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _rentalService.GetAllRentals();
            return Ok(result);
        }
    }
}
