namespace RecipeBookAPI.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }
        required public string Name { get; set; }
        public string? Alergen { get; set; }
        public int? Calories { get; set; }
        public string? ImageURL { get; set; }
    }
}
