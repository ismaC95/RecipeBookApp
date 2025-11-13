namespace RecipeBookAPI.Models
{
    public class RecipeBook
    {
        public int recipeBookID { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string? imageURL { get; set; }
        public bool isPublic { get; set; }
        public DateTime dateCreated { get; set; }

        //FK from User
        public int userID { get; set; }
    }
}
