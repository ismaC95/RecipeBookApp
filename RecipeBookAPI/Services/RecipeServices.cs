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
                throw new InvalidOperationException("You must be logged in");

            var recipe = _recipeRepo.GetByID(recipeID);

            if (recipe is null)
                throw new InvalidOperationException("Recipe not found.");

            //validating user deleting is recipe owner
            if (recipe.OwnerID != user.UserID)
                throw new InvalidOperationException("You are not allowed to delete this recipe.");

            //delete RecipeIngredient rows from this recipe
            var recipeIngredients = _recipeIngredientRepo.GetByRecipeID(recipeID);

            foreach (var ri in recipeIngredients)
            {
                _recipeIngredientRepo.DeleteByIDs(ri.RecipeID, ri.IngredientID);
            }

            //delete the full recipe row
            _recipeRepo.DeleteByID(recipeID);
        }

        public void UpdateRecipe(Recipe updatedRecipe, int userID)
        {
            var existing = _recipeRepo.GetByID(updatedRecipe.RecipeID);
            if (existing == null)
                throw new InvalidOperationException("Recipe not found.");

            if (existing.OwnerID != userID)
                throw new InvalidOperationException("You are not allowed to edit this recipe.");

            //OwnerID and DateCreated won't change
            updatedRecipe.OwnerID = existing.OwnerID;
            updatedRecipe.DateCreated = existing.DateCreated;

            _recipeRepo.Update(updatedRecipe);
        }

        //To update a full recipe we have to as well provide methods to change the ingredients and the subsequent
        //RecipeIngredient junction tables with measures, etc.



        public List<Recipe> GetPublicRecipes()
        {
            var allRecipes = _recipeRepo.GetPublicRecipes();

            if (allRecipes == null) throw new InvalidOperationException("No recipes in the database");
            
            return allRecipes;
        }

        public List<Recipe> GetByOwner(int ownerID)
        {
            if (_recipeRepo.GetByOwner(ownerID) == null) throw new InvalidOperationException("User doesn't exist");
            
            return _recipeRepo.GetByOwner(ownerID);
        }

        //filtering
        public List<Recipe> TotalTimeFilter(int minTime, int maxTime)
        {
            if (minTime < 0 || maxTime < 0)
                throw new ArgumentException("Time values cannot be negative.");

            if (minTime > maxTime)
                throw new ArgumentException("Minimum time cannot be greater than maximum time.");

            var recipes = _recipeRepo.GetPublicRecipes();

            List<Recipe> filteredRecipe = new List<Recipe>();

            foreach (var recipe in recipes)
            {
                int totalTime = recipe.PrepTime + recipe.CookTime;

                if (totalTime >= minTime && totalTime <= maxTime) filteredRecipe.Add(recipe);
            }

            return filteredRecipe;
        }

        public List<Recipe> DifficultyFilter(string difficulty)
        {
            if (string.IsNullOrWhiteSpace(difficulty))
                throw new ArgumentException("Difficulty must be provided.");

            var recipes = _recipeRepo.GetPublicRecipes();

            return recipes
                .Where(r => r.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
