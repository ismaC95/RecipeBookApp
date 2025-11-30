using RecipeBookAPI.Models;
using Microsoft.Data.Sqlite;

namespace RecipeBookAPI.Repositories
{
    public class CategoryRepository
    {
        private const string DbFile = "recipeBook.db";
        public CategoryRepository()
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Categories (
                    CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Description TEXT
                );";
            cmd.ExecuteNonQuery();
        }

        public void Insert(Category c)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Categories
                    (Name, Description)
                VALUES
                    (@name, @desc)";
            cmd.Parameters.AddWithValue("@name", c.Name);
            cmd.Parameters.AddWithValue("@desc", c.Description);

            cmd.ExecuteNonQuery();

        }

        public List<Category> GetAll()
        {
            var list = new List<Category>();
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Category
                {
                    CategoryID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                });
            }

            return list;
        }

        public Category? GetByID(int id)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories WHERE CategoryID = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Category
                {
                    CategoryID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                };
            }

            return null;
        }

        public void DeleteCategory(Category c)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Categories WHERE CategoryID=@id";
            cmd.Parameters.AddWithValue("@id", c.CategoryID);
            cmd.ExecuteNonQuery();
        }

        public Category? GetByName(string name)
        {
            using var connection = new SqliteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories WHERE Name = @n";
            cmd.Parameters.AddWithValue("@n", name);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Category
                {
                    CategoryID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                };
            }

            return null;
        }
    }
}

