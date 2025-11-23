namespace RecipeBookAPI.Models
{
    //Junction table between RecipeBook and Recipe M*M relationship
    public class RecipeBookEntry
    {
        //FK from RecipeBook
        public int RecipeBookID { get; set; }
        //FK from Recipe
        public int RecipeID { get; set; }
        //FK from User
        public int UserID { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
