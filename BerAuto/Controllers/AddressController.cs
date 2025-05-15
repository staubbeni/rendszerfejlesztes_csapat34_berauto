using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BerAuto.Services;
using BerAuto.DataContext.Dtos;
using System.Threading.Tasks;
using System.Security.Claims;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult List()
        {
            var result = _addressService.GetAllAddress();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> Create([FromBody] AddressDto addressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var address = await _addressService.CreateAddressAsync(addressDto, userId);
            return Ok(address);
        }

        [HttpGet("current")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var address = await _addressService.GetAddressByUserIdAsync(userId);

            if (address == null)
                return NotFound("No address found for current user");

            return Ok(address);
        }
    }
}