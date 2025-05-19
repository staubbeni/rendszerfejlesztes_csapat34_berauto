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

            int? userId = null;
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim != null && int.TryParse(idClaim.Value, out var parsedId))
                {
                    userId = parsedId;
                }
            }
            if (!userId.HasValue)
            {
                return Unauthorized();
            }
            var address = await _addressService.CreateAddressAsync(addressDto, userId.Value);
            return Ok(address);
        }

        [HttpGet("current")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
            int? userId = null;
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim != null && int.TryParse(idClaim.Value, out var parsedId))
                {
                    userId = parsedId;
                }
            }
            if (!userId.HasValue)
            {
                return Unauthorized();
            }
            var address = await _addressService.GetAddressByUserIdAsync(userId.Value);

            if (address == null)
                return NotFound("No address found for current user");

            return Ok(address);
        }
    }
}