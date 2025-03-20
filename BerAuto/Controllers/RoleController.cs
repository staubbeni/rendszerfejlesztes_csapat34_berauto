using Microsoft.AspNetCore.Mvc;
using BerAuto.Services;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _roleService.GetAllRoles(); 
            return Ok(result);
        }
    }
}