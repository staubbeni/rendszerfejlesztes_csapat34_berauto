using Microsoft.AspNetCore.Mvc;
using BerAuto.Services;
using BerAuto.DataContext.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BerAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddRole([FromBody] Role role)
        {
            var createdRole = await _roleService.AddRoleAsync(role);
            return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, createdRole);
        }
    }
}