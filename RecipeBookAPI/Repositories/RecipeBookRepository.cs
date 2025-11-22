using RecipeBookAPI.Models;
using System.Data.SQLite;

namespace RecipeBookAPI.Repositories
{
    public class RecipeBookRepository
    {
        private const string DbFile = "recipeBook.db";

        public RecipeBookRepository()
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS RecipeBooks (
                    RecipeBookID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                    Description TEXT,
                    ImageURL TEXT,
                    IsPublic INTEGER NOT NULL,
                    DateCreated TEXT NOT NULL,
                    OwnerID INTEGER NOT NULL
                    
                    FOREIGN KEY (OwnerID) REFERENCES Users(UserID)
                );";
            cmd.ExecuteNonQuery();
        }

        public void Insert(RecipeBook rb)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO RecipeBooks 
                    (Name, Description, ImageURL, IsPublic, DateCreated, OwnerID)
                VALUES
                    (@name, @descr, @img, @public, @date, @owner)";

            cmd.Parameters.AddWithValue("@name", rb.Name);
            cmd.Parameters.AddWithValue("@descr", rb.Description);
            cmd.Parameters.AddWithValue("@img", rb.ImageURL);
            cmd.Parameters.AddWithValue("@public", rb.IsPublic ? 1 : 0);
            cmd.Parameters.AddWithValue("@date", rb.DateCreated.ToString("o"));
            cmd.Parameters.AddWithValue("@owner", rb.OwnerID);

            cmd.ExecuteNonQuery();
        }

        public void Delete(RecipeBook rb)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM RecipeBooks WHERE RecipeBookID=@id";
            cmd.Parameters.AddWithValue("@id", rb.RecipeBookID);
        }

        public void Update(RecipeBook rb)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE RecipeBooks
                SET
                    Name = @name,
                    Description = @descr,
                    ImageURL = @img,
                    OwnerID = @owner,
                    IsPublic = @public
                WHERE
                    RecipeBook = @id";
            cmd.Parameters.AddWithValue("@name", rb.Name);
            cmd.Parameters.AddWithValue("@descr", rb.Description);
            cmd.Parameters.AddWithValue("@img", rb.ImageURL);
            cmd.Parameters.AddWithValue("@owner", rb.OwnerID);
            cmd.Parameters.AddWithValue("@public", rb.IsPublic ? 1 : 0);
            cmd.Parameters.AddWithValue("@id", rb.RecipeBookID);

            cmd.ExecuteNonQuery();
        }

        public List<RecipeBook> GetByOwner(int ownerID)
        {
            var list = new List<RecipeBook>();
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM RecipeBooks WHERE OwnerID=@id";
            cmd.Parameters.AddWithValue("@id", ownerID);


            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new RecipeBook
                {
                    RecipeBookID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    ImageURL = reader.IsDBNull(3) ? null : reader.GetString(3),
                    IsPublic = reader.GetBoolean(4),
                    DateCreated = DateTime.Parse(reader.GetString(5)),
                    OwnerID = reader.GetInt32(6),
                });
            }

            return list;
        }

        public RecipeBook? GetByID(int id)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM RecipeBooks WHERE RecipeBookID = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new RecipeBook
                {
                    RecipeBookID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    ImageURL = reader.IsDBNull(3) ? null : reader.GetString(3),
                    IsPublic = reader.GetBoolean(4),
                    DateCreated = DateTime.Parse(reader.GetString(5)),
                    OwnerID = reader.GetInt32(6),
                };
            }
            return null;
        }
    }
}
