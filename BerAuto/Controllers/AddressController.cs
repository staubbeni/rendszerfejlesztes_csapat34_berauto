using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BerAuto.Services;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _addressService.GetAllAddress();
            return Ok(result);
        }
    }
}
