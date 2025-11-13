using RecipeBookAPI.Models;

namespace RecipeBookAPI.Services
{
    //Recipe class with all the business logic methods
    //When in production, this class receives the requests from Users through controllers and will use the data from the database to provide the controllers with the requested information
    public class RecipeServices
    {
        //Recipe is private so it can't be accesses from outside this class (encapsulation)
        //using a List now but in the future will be a database table with Entitiy Framework (EF Core)
        private static List<Recipe> _recipes = new List<Recipe>();


    }
}
