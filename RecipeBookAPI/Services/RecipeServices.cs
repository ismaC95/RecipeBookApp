using RecipeBookAPI.Models;
using System.Reflection.Metadata.Ecma335;

namespace RecipeBookAPI.Services
{
    //Recipe class with all the business logic methods
    //When in production, this class receives the requests from Users through controllers
    //and will use the data from the database to provide the controllers with the requested information
    public class RecipeServices
    {
        //Recipe is private so it can't be accesses from outside this class (encapsulation)
        //using a List now but in the future will be a database table with Entitiy Framework (EF Core)
        private static List<Recipe> _recipes = new List<Recipe>();

        public Recipe CreateRecipe(Recipe newRecipe)
        {
            //Generic Namespace provides .Count that will count all the items in a list
            newRecipe.recipeID = _recipes.Count + 1;
            newRecipe.dateCreated = DateTime.Now;
            _recipes.Add(newRecipe);

            return newRecipe; 
        }

        public bool DeleteRecipe(int recipeID, int userID)
        {
            //Search for the recipe with recipeID and delete it only if the recipe exists and
            //the user requesting deletion is the owner of the recipe
            var recipe = _recipes.FirstOrDefault(r => r.recipeID == recipeID);
            if (recipe == null) return false;

            var owner = recipe.userID;
            if (owner != userID) return false;

            _recipes.Remove(recipe);

            return true;
        }
    }
}
