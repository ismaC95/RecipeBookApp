namespace RecipeBookAPI.Models
{
    public class RecipeBook
    {
        public int RecipeBookID { get; set; }
        required public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public bool IsPublic { get; set; }
        public DateTime DateCreated { get; set; }

        //FK from User
        public int OwnerID { get; set; }
    }
}
