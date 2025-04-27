using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BerAuto.DataContext.Context;
using System.Linq;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult List()
        {
            var roles = _context.Roles.ToList();
            return Ok(roles);
        }
    }
}
