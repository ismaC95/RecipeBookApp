using Microsoft.AspNetCore.Mvc;
using RecipeBookAPI.Models;
using RecipeBookAPI.Requests;
using RecipeBookAPI.Services;

namespace RecipeBookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : Controller
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly RecipeServices _recipeService;

        public RecipeController(ILogger<RecipeController> logger, RecipeServices recipeService)
        {
            _logger = logger;
            _recipeService = recipeService;
        }

        [HttpPost("Create Recipe")]
        public IActionResult CreateRecipe([FromBody] CreateRecipeRequest request)
        {
            //try
            //{
                var newRecipe = new Recipe
                {
                    Name = request.Recipe.Name,
                    Instructions = request.Recipe.Instructions,
                    PrepTime = request.Recipe.PrepTime,
                    CookTime = request.Recipe.CookTime,
                    Difficulty = request.Recipe.Difficulty,
                    IsPublic = request.Recipe.IsPublic,
                    ImageURL = request.Recipe.ImageURL,
                    OwnerID = request.Recipe.OwnerID,
                    CategoryID = request.Recipe.CategoryID,
                };
                var ingredients = new List<Ingredient>();

                foreach (var ingReq in request.Ingredients)
                {
                    ingredients.Add(new Ingredient
                    {
                        Name = ingReq.Name,
                        Alergen = ingReq.Alergen,
                        Calories = ingReq.Calories,
                        ImageURL = ingReq.ImageURL
                    });
                }

                var measures = new List<RecipeIngredient>();

                foreach (var mReq in request.IngredientMeasures)
                {
                    measures.Add(new RecipeIngredient
                    {
                        Quantity = mReq.Quantity,
                        UnitOfMeasure = mReq.UnitOfMeasure
                    });
                }

                _recipeService.CreateRecipe(
                        newRecipe,
                        measures,
                        ingredients
                    );

                return Ok("Recipe created successfully.");
            //}
            //catch (Exception ex)
            //{
            //    return Conflict(ex.Message);
            //}
        }
    }
}

