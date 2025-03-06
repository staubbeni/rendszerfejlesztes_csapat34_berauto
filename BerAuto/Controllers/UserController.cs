using Microsoft.AspNetCore.Mvc;
using BerAuto.Service;

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
            var result = _userService.GetAllUser();
            return Ok(result);
        }
    }
}