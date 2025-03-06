using Microsoft.AspNetCore.Mvc;
using BerAuto.Service;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService RoleService)
        {
            _roleService = RoleService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _carService.GetAllRoles();
            return Ok(result);
        }
    }
}