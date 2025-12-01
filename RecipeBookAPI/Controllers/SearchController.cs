using Microsoft.AspNetCore.Mvc;
using RecipeBookAPI.Models;
using RecipeBookAPI.Services;

namespace RecipeBookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly SearchServices _searchService;
        private readonly ILogger<SearchController> _logger;
        public SearchController(SearchServices searchService, ILogger<SearchController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        [HttpGet("recipes")]
        public IActionResult SearchRecipe([FromQuery] string keyword)
        {
            try
            {
                List<Recipe> results = _searchService.RecipeLinearSearch(keyword);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
