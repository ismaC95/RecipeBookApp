namespace RecipeBookAPI.Models
{
    //Junction table between RecipeBook and Recipe M*M relationship
    public class RecipeBookEntry
    {
        //FK from RecipeBook
        public int recipeBookID { get; set; }
        //FK from Recipe
        public int recipeID { get; set; }
        //FK from User
        public int userID { get; set; }
        public DateTime dateAdded { get; set; }
    }
}
