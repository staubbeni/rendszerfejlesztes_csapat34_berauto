using BerAuto.Services;
using BerAuto.DataContext.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BerAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {
            var user = await _userService.RegisterAsync(userDto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            var token = await _userService.LoginAsync(userDto);
            return Ok(new { token });
        }

        [HttpPut("update-profile/{userId}")]
        public async Task<IActionResult> UpdateProfile(int userId, UserUpdateDto userDto)
        {
            var updatedUser = await _userService.UpdateProfileAsync(userId, userDto);
            return Ok(updatedUser);
        }

        [HttpPut("update-address/{userId}")]
        public async Task<IActionResult> UpdateAddress(int userId, AddressDto addressDto)
        {
            var updatedUser = await _userService.UpdateAddressAsync(userId, addressDto);
            return Ok(updatedUser);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _userService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
