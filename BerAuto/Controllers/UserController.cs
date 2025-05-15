using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BerAuto.Services;
using BerAuto.DataContext.Dtos;
using System.Threading.Tasks;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;

        public UserController(IUserService userService, IAddressService addressService)
        {
            _userService = userService;
            _addressService = addressService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {
            var user = await _userService.RegisterAsync(userDto);
            return Ok(user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            var token = await _userService.LoginAsync(userDto);
            return Ok(new { token });
        }

        [HttpPut("update-profile/{userId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateProfile(int userId, UserUpdateDto userDto)
        {
            var updatedUser = await _userService.UpdateProfileAsync(userId, userDto);
            return Ok(updatedUser);
        }

        [HttpPut("update-address/{userId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateAddress(int userId, AddressDto addressDto)
        {
            // Frissítjük a címet az AddressService használatával
            await _addressService.CreateAddressAsync(addressDto, userId);

            // Visszaadjuk a frissített felhasználót
            var updatedUser = await _userService.GetUserByIdAsync(userId);
            return Ok(updatedUser);
        }

        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _userService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}