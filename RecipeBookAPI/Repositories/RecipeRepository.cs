using Microsoft.Data.Sqlite;
using RecipeBookAPI.Models;

namespace RecipeBookAPI.Repositories
{
    public class RecipeRepository
    {
        private const string DbFile = "recipeBook.db";

        public RecipeRepository() {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Recipes (
                    RecipeID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Instructions TEXT NOT NULL,
                    PrepTime INTEGER NOT NULL,
                    CookTime INTEGER NOT NULL,
                    Difficulty TEXT NOT NULL,
                    DateCreated TEXT NOT NULL,
                    IsPublic INTEGER NOT NULL,
                    ImageURL TEXT,
                    OwnerID INTEGER NOT NULL,
                    CategoryID  INTEGER NOT NULL

                    FOREIGN KEY (OwnerID) REFERENCES Users(UserID),
                    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
                );";
            cmd.ExecuteNonQuery();
        }
        public int Insert(Recipe r)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Recipes 
                    (Name, Instructions, PrepTime, CookTime, Difficulty, DateCreated, IsPublic, ImageURL, OwnerID, CategoryID) 
                VALUES 
                    (@name, @instr, @prep, @cook, @diff, @date, @public, @img, @owner, @cat)";

            cmd.Parameters.AddWithValue("@name", r.Name);
            cmd.Parameters.AddWithValue("@instr", r.Instructions);
            cmd.Parameters.AddWithValue("@prep", r.PrepTime);
            cmd.Parameters.AddWithValue("@cook", r.CookTime);
            cmd.Parameters.AddWithValue("@diff", r.Difficulty);
            //There's no dateTime data type in SQLite, it's standard to store it as a string "o" format to
            //guarantee a satable, safe and universal date string
            cmd.Parameters.AddWithValue("@date", r.DateCreated.ToString("o"));
            //IsPublic is a boolean, therefore we have to introduce it as such
            cmd.Parameters.AddWithValue("@public", r.IsPublic ? 1 : 0);
            cmd.Parameters.AddWithValue("@img", r.ImageURL);
            cmd.Parameters.AddWithValue("@owner", r.OwnerID);
            cmd.Parameters.AddWithValue("@cat", r.CategoryID);

            cmd.ExecuteNonQuery();

            //To provide the FK to junction tables we need to return the RecipeID
            var idCmd = connection.CreateCommand();
            idCmd.CommandText = "SELECT last_insert_rowid();";

            return Convert.ToInt32(idCmd.ExecuteScalar());
        }

        public void Delete(Recipe r)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Recipes WHERE RecipeID=@id";
            cmd.Parameters.AddWithValue("@id", r.RecipeID);
            cmd.ExecuteNonQuery();
        }

        public void Update(Recipe r)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE Recipes
                SET
                    Name = @name,
                    Instructions = @instr,
                    PrepTime = @prep,
                    CookTime = @cook,
                    Difficulty = @diff,
                    IsPublic = @public,
                    ImageURL = @img,
                    OwnerID = @owner,
                    CategoryID = @cat
                WHERE
                    RecipeID = @id";
            cmd.Parameters.AddWithValue("@name", r.Name);
            cmd.Parameters.AddWithValue("@instr", r.Instructions);
            cmd.Parameters.AddWithValue("@prep", r.PrepTime);
            cmd.Parameters.AddWithValue("@cook", r.CookTime);
            cmd.Parameters.AddWithValue("@diff", r.Difficulty);
            cmd.Parameters.AddWithValue("@public", r.IsPublic ? 1 : 0);
            cmd.Parameters.AddWithValue("@img", r.ImageURL);
            cmd.Parameters.AddWithValue("@owner", r.OwnerID);
            cmd.Parameters.AddWithValue("@cat", r.CategoryID);
            cmd.Parameters.AddWithValue("@id", r.RecipeID);

            cmd.ExecuteNonQuery();
        }

        public List<Recipe> GetAll()
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();
            var list = new List<Recipe>();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Recipes";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Recipe
                {
                    RecipeID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Instructions = reader.GetString(2),
                    PrepTime = reader.GetInt32(3),
                    CookTime = reader.GetInt32(4),
                    Difficulty = reader.GetString(5),
                    DateCreated = DateTime.Parse(reader.GetString(6)),
                    IsPublic = reader.GetBoolean(7),
                    //IsDBNull to avoid exceptions if there's no imageURL
                    ImageURL = reader.IsDBNull(8) ? null : reader.GetString(8),
                    OwnerID = reader.GetInt32(9),
                    CategoryID = reader.GetInt32(10),
                });
            }

            return list;
        }

        public Recipe? GetByID(int id)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Recipes WHERE RecipeID = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Recipe
                {
                    RecipeID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Instructions = reader.GetString(2),
                    PrepTime = reader.GetInt32(3),
                    CookTime = reader.GetInt32(4),
                    Difficulty = reader.GetString(5),
                    DateCreated = DateTime.Parse(reader.GetString(6)),
                    IsPublic = reader.GetBoolean(7),
                    ImageURL = reader.IsDBNull(8) ? null : reader.GetString(8),
                    OwnerID = reader.GetInt32(9),
                    CategoryID = reader.GetInt32(10),
                };
            }
            return null;
        }

        public List<Recipe> GetPublicRecipes()
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Recipes WHERE IsPublic = 1";

            var list = new List<Recipe>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Recipe
                {
                    RecipeID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Instructions = reader.GetString(2),
                    PrepTime = reader.GetInt32(3),
                    CookTime = reader.GetInt32(4),
                    Difficulty = reader.GetString(5),
                    DateCreated = DateTime.Parse(reader.GetString(6)),
                    IsPublic = reader.GetBoolean(7),
                    ImageURL = reader.IsDBNull(8) ? null : reader.GetString(8),
                    OwnerID = reader.GetInt32(9),
                    CategoryID = reader.GetInt32(10),
                });
            }

            return list;
        }

        public List<Recipe> GetByCategory(int categoryID)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Recipes WHERE CategoryID=@cat";
            cmd.Parameters.AddWithValue("@cat", categoryID);


            var list = new List<Recipe>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Recipe
                {
                    RecipeID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Instructions = reader.GetString(2),
                    PrepTime = reader.GetInt32(3),
                    CookTime = reader.GetInt32(4),
                    Difficulty = reader.GetString(5),
                    DateCreated = DateTime.Parse(reader.GetString(6)),
                    IsPublic = reader.GetBoolean(7),
                    ImageURL = reader.IsDBNull(8) ? null : reader.GetString(8),
                    OwnerID = reader.GetInt32(9),
                    CategoryID = reader.GetInt32(10),
                });
            }

            return list;
        }
    }
}
