namespace RecipeBookAPI.Requests
{
    public class CreateRecipeRequest
    {
        public required CreateRecipeInfoRequest Recipe { get; set; }
        required public List<CreateIngredientRequest> Ingredients { get; set; }
        required public List<CreateIngredientMeasureRequest> IngredientMeasures { get; set; }
    }
    public class CreateRecipeInfoRequest
    {
        required public string Name { get; set; }
        required public string Instructions { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        required public string Difficulty { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsPublic { get; set; }
        //imageURL datatype has ? because it can be NULL (Nullable Reference Types)
        public string? ImageURL { get; set; }
        //FK from User
        public int OwnerID { get; set; }
        //FK from Category
        public int CategoryID { get; set; }
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
        public decimal Quantity { get; set; }
        required public string UnitOfMeasure { get; set; }
    }
}
