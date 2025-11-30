using RecipeBookAPI.Models;
using RecipeBookAPI.Repositories;

namespace RecipeBookAPI.Services
{
    public class SearchServices
    {
        private readonly RecipeRepository _recipeRepo;
        private readonly IngredientRepository _ingredientRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly RecipeIngredientRepository _recipeIngredientRepo;

        public SearchServices(
            RecipeRepository recipeRepo, 
            IngredientRepository ingredientRepo, 
            CategoryRepository categoryRepo,
            RecipeIngredientRepository recipeIngredientRepo)
        {
            _recipeRepo = recipeRepo;
            _ingredientRepo = ingredientRepo;
            _categoryRepo = categoryRepo;
            _recipeIngredientRepo = recipeIngredientRepo;
        }

        //This service class searchs recipes by recipe name, ingredient name and category name and provides
        //a list of all the distinct recipes that matches with the user provided keyword
        public List<Recipe> RecipeLinearSearch(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) throw new Exception("Missing search keyword");

            var finalResults = new List<Recipe>();
            
            finalResults.AddRange(SearchRecipeByName(keyword));

            finalResults.AddRange(SearchRecipeByCategory(keyword));

            finalResults.AddRange(SearchRecipeByIngredient(keyword));

            return finalResults.DistinctBy(r => r.RecipeID).ToList();
        }

        public List<Recipe> SearchRecipeByName(string keyword)
        {
            var publicRepices = _recipeRepo.GetPublicRecipes();

            var matchedRecipes = new List<Recipe>();

            foreach (var recipe in publicRepices)
            {
                if(recipe.Name.Contains(keyword,StringComparison.OrdinalIgnoreCase)) matchedRecipes.Add(recipe);
            }

            return matchedRecipes;
        }

        public List<Recipe> SearchRecipeByCategory(string keyword)
        {
            var categories = _categoryRepo.GetAll();
            var results = new List<Recipe>();

            foreach (var category in categories)
            {
                if (category.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)) 
                {
                    var recipesWithMatchedCategory = _recipeRepo.GetByCategory(category.CategoryID);
                    results.AddRange(recipesWithMatchedCategory);
                }
            }
            return results;
        }

        public List<Recipe> SearchRecipeByIngredient(string keyword)
        {
            var ingredients = _ingredientRepo.GetAll();
            var results = new List<Recipe>();

            foreach(var ingredient in ingredients)
            {
                if(ingredient.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    var recipeIDs = _recipeIngredientRepo.GetRecipesIDByIngredient(ingredient.IngredientID);

                    foreach(var id in recipeIDs)
                    {
                        var recipe = _recipeRepo.GetByID(id);
                        if (recipe != null)
                            results.Add(recipe);
                    }
                }
            }
            return results;
        }

    }
}
