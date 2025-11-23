using RecipeBookAPI.Models;
using System.Data.SQLite;

namespace RecipeBookAPI.Repositories
{
    public class RecipeBookEntryRepository
    {
        private const string DbFile = "recipeBook.db";

        public RecipeBookEntryRepository()
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS RecipeBookEntries (
                    RecipeBookID INTEGER NOT NULL,
                    RecipeID INTEGER NOT NULL,
                    UserID INTEGER NOT NULL,
                    DateAdded TEXT NOT NULL,
                PRIMARY KEY (RecipeBookID, RecipeID),
                FOREIGN KEY (RecipeBookID) REFERENCES RecipeBook(RecipeBookID),
                FOREIGN KEY (RecipeID) REFERENCES Recipes(RecipeID),
                FOREIGN KEY (UserID) REFERENCES User(UserID)
                );";
            cmd.ExecuteNonQuery();
        }

        public void Insert(RecipeBookEntry rbe)
        {
            using var connection = new SQLiteConnection($"Data Source={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO RecipeBookEntries
                    (RecipeBookID, RecipeID, UserID, DateAdded)
                VALUES
                    (@rb, @recipe, @user, @date)";
            cmd.Parameters.AddWithValue("@rb", rbe.RecipeBookID);
            cmd.Parameters.AddWithValue("@recipe", rbe.RecipeID);
            cmd.Parameters.AddWithValue("@user", rbe.UserID);
            cmd.Parameters.AddWithValue("@date", rbe.DateAdded.ToString("o"));

            cmd.ExecuteNonQuery();
        }

        public void Delete(RecipeBookEntry rbe)
        {
            using var connection = new SQLiteConnection($"Data Source={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM RecipeBookEntries WHERE RecipeBookID = @rb AND RecipeID = @recipe";
            cmd.Parameters.AddWithValue("@rb", rbe.RecipeBookID);
            cmd.Parameters.AddWithValue("@recipe", rbe.RecipeID);

            cmd.ExecuteNonQuery();
        }
        
        //Get all the recipes from a RecipeBook
        public List<RecipeBookEntry> GetByRecipeBook(int id)
        {
            var list = new List<RecipeBookEntry>();

            using var connection = new SQLiteConnection($"Data Source={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT RecipeBookID, RecipeID, UserID, DateAdded
                FROM RecipeBookEntries
                WHERE RecipeBookID = @rb";
            cmd.Parameters.AddWithValue("@rb", id);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new RecipeBookEntry
                {
                    RecipeBookID = reader.GetInt32(0),
                    RecipeID = reader.GetInt32(1),
                    UserID = reader.GetInt32(2),
                    DateAdded = DateTime.Parse(reader.GetString(3))
                });
            }

            return list;
        }
    }
}

