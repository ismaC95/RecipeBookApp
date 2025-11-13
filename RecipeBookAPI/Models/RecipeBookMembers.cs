namespace RecipeBookAPI.Models
{
    //Junction table between User and RecipeBook tables
    public class RecipeBookMembers
    {
        //FK from User
        public int userID { get; set; }
        //FK from Recipe
        public int recipeID { get; set; }
        public DateTime joinedDate { get; set; }
    }
}
