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

        [HttpPost("CreateRecipe")]
        public IActionResult CreateRecipe([FromBody] CreateRecipeRequest request)
        {
            try
            {
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

                return Ok("Recipe created");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{recipeID}")]
        public IActionResult DeleteRecipe(int recipeID, [FromQuery] int userID)
        {
            try
            {
                _recipeService.DeleteRecipe(recipeID, userID);
                return Ok("Recipe deleted");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("UpdateRecipe")]
        public IActionResult UpdateRecipe([FromBody] Recipe recipe, [FromQuery] int userID)
        {
            try
            {
                _recipeService.UpdateRecipe(recipe, userID);
                return Ok("Recipe updated");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("getRecipes/all")]
        public IActionResult GetAllRecipes()
        {
            try
            {
                var recipes = _recipeService.GetPublicRecipes();
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getRecipes/byUser")]
        public IActionResult GetRecipeByUser(int ownerID)
        {
            try
            {
                var recipes = _recipeService.GetByOwner(ownerID);
                return Ok(recipes);
            } catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("filter/totalTime")]
        public IActionResult FilterByTotalTime(int minTime, int maxTime)
        {
            try
            {
                var results = _recipeService.TotalTimeFilter(minTime, maxTime);
                return Ok(results);
            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("filter/difficulty")]
        public IActionResult FilterByDifficulty([FromQuery] string difficulty)
        {
            return Ok(_recipeService.DifficultyFilter(difficulty));
        }
    }
}

