namespace RecipeBookAPI.Models
{
    public class Recipe
    {
        //Represents Recipe entity stored in the database
        public int recipeID { get; set; }
        public string name { get; set; }
        public string instructions { get; set; }
        public int prepTime { get; set; }
        public int cookTime { get; set; }
        public string difficulty { get; set; }
        public DateTime dateCreated { get; set; }
        public bool isPublic { get; set; }
        //imageURL datatype has ? because it can be NULL (Nullable Reference Types)
        public string? imageURL { get; set; }
        public int ownerID { get; set; }
    }
}
