using Microsoft.AspNetCore.Mvc;
using BerAuto.Services;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService UserService)
        {
            _userService = UserService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _userService.GetAllUsers();
            return Ok(result);
        }
    }
}