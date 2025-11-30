using RecipeBookAPI.Models;
using Microsoft.Data.Sqlite;
using System.Reflection.Metadata;

namespace RecipeBookAPI.Repositories
{
    public class IngredientRepository
    {
        private const string DbFile = "recipeBook.db";
        public IngredientRepository() 
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Ingredients (
                    IngredientID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Alergen TEXT,
                    Calories INTEGER,
                    ImageURL TEXT
                );";
            cmd.ExecuteNonQuery();
        }

        public int Insert(Ingredient i)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Ingredients
                    (Name, Alergen, Calories, ImageURL)
                VALUES
                    (@name, @alergen, @cal, @img)";
            cmd.Parameters.AddWithValue("@name", i.Name);
            cmd.Parameters.AddWithValue("@alergen", i.Alergen as object ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cal", i.Calories);
            cmd.Parameters.AddWithValue("@img", i.ImageURL as object ?? DBNull.Value);

            cmd.ExecuteNonQuery();

            //To provide the FK to junction tables we need to return the RecipeID
            var idCmd = connection.CreateCommand();
            idCmd.CommandText = "SELECT last_insert_rowid();";

            return Convert.ToInt32(idCmd.ExecuteScalar());
        }

        public List<Ingredient> GetAll()
        {
            var list = new List<Ingredient>();
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ingredients";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Ingredient
                {
                    IngredientID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Alergen = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Calories = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    ImageURL = reader.IsDBNull(4) ? null : reader.GetString(4),
                });
            }

            return list;
        }
        public Ingredient? GetByID(int id)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ingredients WHERE IngredientID = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Ingredient
                {
                    IngredientID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Alergen = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Calories = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    ImageURL = reader.IsDBNull(4) ? null : reader.GetString(4),
                };
            }
            return null;
        }

        public Ingredient? GetByName(string name)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT IngredientID, Name FROM Ingredients WHERE Name = @n";
            cmd.Parameters.AddWithValue("@n", name);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Ingredient
                {
                    IngredientID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                };
            }
            return null;
        }
    }
}
