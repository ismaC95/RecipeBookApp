using RecipeBookAPI.Models;
using RecipeBookAPI.Repositories;

namespace RecipeBookAPI.Services
{
    //Recipe class with all the business logic methods
    //When in production, this class receives the requests from Users through controllers
    //and will use the data from the database to provide the controllers with the requested information
    public class RecipeServices
    {
        private readonly RecipeRepository _repo;
        private readonly UserRepository _userRepo;
        private readonly IngredientRepository _ingredientRepo;
        private readonly RecipeIngredientRepository _recipeIngredientRepo;

        public RecipeServices(
            RecipeRepository recipeRepo, 
            UserRepository userRepo, 
            IngredientRepository ingredientRepo,
            RecipeIngredientRepository recipeIngredientRepo)
        {
            _repo = recipeRepo;
            _userRepo = userRepo;
            _ingredientRepo = ingredientRepo;
            _recipeIngredientRepo = recipeIngredientRepo;
        }

        //public void CreateRecipe(
        //    Recipe recipe,
        //    List<RecipeIngredient> ingredients)
        //{
        //    var user = _userRepo.GetByID(recipe.OwnerID);
        //    if (user is null)
        //        throw new Exception("You must be logged in");

        //    recipe.DateCreated = DateTime.Now;   
        //    _repo.Insert(recipe);

        //    foreach(var ingredient in ingredients)
        //    {
        //        var ingredient = _ingredientRepo
        //    }
        //}

        public void DeleteRecipe(int recipeID, int userID)
        {
            var user = _userRepo.GetByID(userID);
            if (user is null)
                throw new Exception("You must be logged in");

            var recipe = _repo.GetByID(recipeID);

            if (recipe is null)
                throw new Exception("Recipe not found.");

            //validating user deleting is recipe owner
            if (recipe.OwnerID != user.UserID)
                throw new Exception("You are not allowed to delete this recipe.");

            _repo.Delete(recipe);
        }

        public List<Recipe> GetPublicRecipes()
        {
            return _repo.GetPublicRecipes();
        }
    }
}
