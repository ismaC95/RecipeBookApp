namespace RecipeBookAPI.Models
{
    //Junction table between Recipe and Ingredient M*M relationship
    public class RecipeIngredient
    {
        //FK from Recipe
        public int recipeID { get; set; }
        //FK from Ingredient
        public int ingredientID { get; set; }
        public int quantity { get; set; }
        public string unitOfMeasure { get; set; }
    }
}
