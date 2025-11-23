using RecipeBookAPI.Models;
using System.Data.SQLite;

namespace RecipeBookAPI.Repositories
{
    public class RecipeIngredientRepository
    {
        private const string DbFile = "recipeBook.db";

        public RecipeIngredientRepository()
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS RecipeIngredients (
                    RecipeID INTEGER NOT NULL,
                    IngredientID INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    UnitOfMeasure TEXT NOT NULL,
                PRIMARY KEY (RecipeID, IngredientID),
                FOREIGN KEY (RecipeID) REFERENCES Recipes(RecipeID),
                FOREIGN KEY (IngredientID) REFERENCES Ingredients(IngredientID)
                );";
            cmd.ExecuteNonQuery();
        }

        public void Insert(RecipeIngredient ri)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO RecipeIngredients
                    (RecipeID, IngredientID, Quantity, UnitOfMeasure)
                VALUES
                    (@recipe, @ingr, @quantity, @unit)";
            cmd.Parameters.AddWithValue("@recipe", ri.RecipeID);
            cmd.Parameters.AddWithValue("@ingr", ri.IngredientID);
            cmd.Parameters.AddWithValue("@quantity", ri.Quantity);
            cmd.Parameters.AddWithValue("@unit", ri.UnitOfMeasure);

            cmd.ExecuteNonQuery();
        }

        public void Delete(RecipeIngredient ri)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM RecipeIngredients
                WHERE RecipeID = @recipe AND IngredientID = @ingr";

            cmd.Parameters.AddWithValue("@recipe", ri.RecipeID);
            cmd.Parameters.AddWithValue("@ingr", ri.IngredientID);

            cmd.ExecuteNonQuery();
        }

        public List<RecipeIngredient> GetByRecipeID(int recipeID)
        {
            var list = new List<RecipeIngredient>();

            using var connection = new SQLiteConnection($"Data Source={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT RecipeID, IngredientID, Quantity, UnitOfMeasure
                FROM RecipeIngredients
                WHERE RecipeID = @recipe";

            cmd.Parameters.AddWithValue("@recipe", recipeID);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new RecipeIngredient
                {
                    RecipeID = reader.GetInt32(0),
                    IngredientID = reader.GetInt32(1),
                    Quantity = reader.GetInt32(2),
                    UnitOfMeasure = reader.GetString(3)
                });
            }

            return list;
        }

    }
}
