using Microsoft.AspNetCore.Mvc;
using BerAuto.Services;
using BerAuto.DataContext.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BerAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> AddAddress([FromBody] Address address)
        {
            var createdAddress = await _addressService.AddAddressAsync(address);
            return CreatedAtAction(nameof(GetAddress), new { id = createdAddress.Id }, createdAddress);
        }

        [HttpGet("user/{userId}")]
        //[Authorize]
        public async Task<IActionResult> GetAddress(int userId)
        {
            var address = await _addressService.GetAddressByUserIdAsync(userId);
            if (address == null) return NotFound();
            return Ok(address);
        }

        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] Address address)
        {
            if (id != address.Id) return BadRequest();
            await _addressService.UpdateAddressAsync(address);
            return NoContent();
        }
    }
}