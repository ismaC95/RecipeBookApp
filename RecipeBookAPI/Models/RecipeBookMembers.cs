namespace RecipeBookAPI.Models
{
    //Junction table between User and RecipeBook tables
    public class RecipeBookMember
    {
        //FK from User
        public int UserID { get; set; }
        //FK from Recipe
        public int RecipeBookID { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}
