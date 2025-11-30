using RecipeBookAPI.Models;
using RecipeBookAPI.Repositories;

namespace RecipeBookAPI.Services
{
    //Recipe class with all the business logic methods
    //When in production, this class receives the requests from Users through controllers
    //and will use the data from the database to provide the controllers with the requested information
    public class RecipeServices
    {
        private readonly RecipeRepository _recipeRepo;
        private readonly UserRepository _userRepo;
        private readonly IngredientRepository _ingredientRepo;
        private readonly RecipeIngredientRepository _recipeIngredientRepo;

        public RecipeServices(
            RecipeRepository recipeRepo,
            UserRepository userRepo,
            IngredientRepository ingredientRepo,
            RecipeIngredientRepository recipeIngredientRepo)
        {
            _recipeRepo = recipeRepo;
            _userRepo = userRepo;
            _ingredientRepo = ingredientRepo;
            _recipeIngredientRepo = recipeIngredientRepo;
        }

        public int CreateRecipe(
            Recipe recipe,
            List<RecipeIngredient> ingredientMeasures,
            List<Ingredient> ingredients)
        {
            //is the user registered?
            var user = _userRepo.GetByID(recipe.OwnerID);
            if (user is null)
                throw new InvalidOperationException("You must be logged in");

            //create new Recipe in RecipeRepository with date of today
            recipe.DateCreated = DateTime.Now;
            int newRecipeID = _recipeRepo.Insert(recipe);

            //From all ingredients shared by the user, check if they are in the database, if they aren't then
            //add them
            List<int> resolvedIngredientsIDs = new List<int>();

            foreach (var ingredient in ingredients)
            {
                int ingredientID;

                var existingIngredient = _ingredientRepo.GetByName(ingredient.Name);

                if (existingIngredient == null)
                {
                    ingredientID = _ingredientRepo.Insert(ingredient);
                }
                else
                {
                    ingredientID = existingIngredient.IngredientID;
                }

                resolvedIngredientsIDs.Add(ingredientID);
            }

            //For all the measures from each ingredient, get the recipeID and ingredientID and populate the junction
            //table 
            for (int i = 0; i < ingredientMeasures.Count; i++)
            {
                var ri = ingredientMeasures[i];

                ri.RecipeID = newRecipeID;
                ri.IngredientID = resolvedIngredientsIDs[i];

                _recipeIngredientRepo.Insert(ri);
            }

            return newRecipeID;
        }

        public void DeleteRecipe(int recipeID, int userID)
        {
            var user = _userRepo.GetByID(userID);
            if (user is null)
                throw new Exception("You must be logged in");

            var recipe = _recipeRepo.GetByID(recipeID);

            if (recipe is null)
                throw new Exception("Recipe not found.");

            //validating user deleting is recipe owner
            if (recipe.OwnerID != user.UserID)
                throw new Exception("You are not allowed to delete this recipe.");

            _recipeRepo.Delete(recipe);
        }

        public List<Recipe> GetPublicRecipes()
        {
            return _recipeRepo.GetPublicRecipes();
        }
    }
}
