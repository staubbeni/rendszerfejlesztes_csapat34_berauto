using Microsoft.AspNetCore.Mvc;
using BerAuto.Service;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CarCategoryController : ControllerBase
    {
        private readonly ICarCategoryService _carCategoryService;

        public CarCategoryController(ICarCategoryService CarCategoryService)
        {
            _carCategoryService = CarCategoryService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _carCategoryService.GetAllCarCategories();
            return Ok(result);
        }
    }
}