namespace RecipeBookAPI.Models
{
    //Represents Recipe entity stored in the database
    public class Recipe
    {
        public int RecipeID { get; set; }
        required public string Name { get; set; }
        required public string Instructions { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public string? Difficulty { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsPublic { get; set; }
        //imageURL datatype has ? because it can be NULL (Nullable Reference Types)
        public string? ImageURL { get; set; }
        //FK from User
        public int OwnerID { get; set; }
        //FK from Category
        public int CategoryID { get; set; }
    }
}
