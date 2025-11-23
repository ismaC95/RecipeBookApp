namespace RecipeBookAPI.Models
{
    //Junction table between Recipe and Ingredient M*M relationship
    public class RecipeIngredient
    {
        //FK from Recipe
        public int RecipeID { get; set; }
        //FK from Ingredient
        public int IngredientID { get; set; }
        public int Quantity { get; set; }
        required public string UnitOfMeasure { get; set; }
    }
}
