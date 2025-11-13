namespace RecipeBookAPI.Models
{
    public class Ingredient
    {
        public int ingredientID { get; set; }
        public string name { get; set; }
        public string? alergen { get; set; }
        public int calories { get; set; }
    }
}
