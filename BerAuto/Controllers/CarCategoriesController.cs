using Microsoft.AspNetCore.Mvc;
using BerAuto.Services;
using BerAuto.DataContext.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BerAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarCategoriesController : ControllerBase
    {
        private readonly ICarCategoryService _categoryService;

        public CarCategoriesController(ICarCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddCategory([FromBody] CarCategory category)
        {
            var createdCategory = await _categoryService.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CarCategory category)
        {
            if (id != category.Id) return BadRequest();
            await _categoryService.UpdateCategoryAsync(category);
            return NoContent();
        }
    }
}