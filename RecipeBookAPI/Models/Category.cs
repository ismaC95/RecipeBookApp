namespace RecipeBookAPI.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        required public string Name { get; set; }
        public string? Description { get; set; }
    }
}
