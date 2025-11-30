using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeBookAPI.Models;
using RecipeBookAPI.Services;

namespace RecipeBookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly CategoryServices _categoryService;

        public CategoryController(ILogger<CategoryController> logger, CategoryServices categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpPost("CreateCategory")]
        public IActionResult CreateCategory(string name, string description)
        {
            try
            {
                var newCategory = new Category
                {
                    Name = name,
                    Description = description
                };

                _categoryService.CreateCategory(newCategory);
                return Ok();
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(ex.Message);
            }
            
            
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null) return NotFound($"Category with ID {id} not found.");
            return Ok(category);
        }

        [HttpGet("Get all Categories")]
        public List<Category> GetCategories()
        {
            return _categoryService.GetAllCategories();
        }

        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category != null)
            {
                _categoryService.DeleteCategory(category);
                return Ok(category);
            }
            return BadRequest("Missing Category to delete");
        }
    }
}
