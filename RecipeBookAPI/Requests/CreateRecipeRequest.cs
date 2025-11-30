using RecipeBookAPI.Models;

namespace RecipeBookAPI.Requests
{
    public class CreateRecipeRequest
    {
        public Recipe? Recipe { get; set; }
        required public List<CreateIngredientRequest> Ingredients { get; set; }
        required public List<CreateIngredientMeasureRequest> IngredientMeasures { get; set; }
    }
    public class CreateIngredientRequest
    {
        required public string Name { get; set; }
        public string? Alergen { get; set; }
        public int? Calories { get; set; }
        public string? ImageURL { get; set; }
    }
    public class CreateIngredientMeasureRequest
    {
        public int Quantity { get; set; }
        required public string UnitOfMeasure { get; set; }
    }
}
