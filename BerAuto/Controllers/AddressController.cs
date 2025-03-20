using Microsoft.AspNetCore.Mvc;
using BerAuto.Services;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService AddressService)
        {
            _addressService = AddressService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _addressService.GetAllAddress();
            return Ok(result);
        }
    }
}