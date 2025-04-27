using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BerAuto.Services;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CarCategoryController : ControllerBase
    {
        private readonly ICarCategoryService _carCategoryService;

        public CarCategoryController(ICarCategoryService carCategoryService)
        {
            _carCategoryService = carCategoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult List()
        {
            var result = _carCategoryService.GetAllCarCategories();
            return Ok(result);
        }
    }
}
