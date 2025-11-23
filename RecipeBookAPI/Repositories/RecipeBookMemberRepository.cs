using System.Data.SQLite;
using RecipeBookAPI.Models;

namespace RecipeBookAPI.Repositories
{
    public class RecipeBookMemberRepository
    {
        private const string DbFile = "recipeBook.db";
        public RecipeBookMemberRepository()
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS RecipeBookMembers (
                    UserID INTEGER NOT NULL,
                    RecipeBookID INTEGER NOT NULL,
                    JoinedDate TEXT NOT NULL,
                PRIMARY KEY (UserID, RecipeBookID),
                FOREIGN KEY (UserID) REFERENCES Users(UserID),
                FOREIGN KEY (RecipeBookID) REFERENCES RecipeBooks(RecipeBookID)
            );";
            cmd.ExecuteNonQuery();
        }

        public void Insert(RecipeBookMember rbm)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO RecipeBookMembers
                    (UserID, RecipeBookID, JoinedDate)
                VALUES
                    (@user, @rb, @date)";
            cmd.Parameters.AddWithValue("@user", rbm.UserID);
            cmd.Parameters.AddWithValue("@rb", rbm.RecipeBookID);
            cmd.Parameters.AddWithValue("@date", rbm.JoinedDate.ToString("o"));

            cmd.ExecuteNonQuery();
        }

        public void Delete(RecipeBookMember rbm)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM RecipeBookMembers
                WHERE UserID = @user AND RecipeBookID = @rb";

            cmd.Parameters.AddWithValue("@user", rbm.UserID);
            cmd.Parameters.AddWithValue("@rb", rbm.RecipeBookID);

            cmd.ExecuteNonQuery();
        }

        //Get all members of a RecipeBook
        public List<RecipeBookMember> GetByBook(int RecipeBookID)
        {
            var list = new List<RecipeBookMember>();

            using var connection = new SQLiteConnection($"Data Source={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM RecipeBookMembers WHERE RecipeBookID = @rb";

            cmd.Parameters.AddWithValue("@rb", RecipeBookID);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new RecipeBookMember
                {
                    UserID = reader.GetInt32(0),
                    RecipeBookID = reader.GetInt32(1),
                    JoinedDate = DateTime.Parse(reader.GetString(2))
                });
            }

            return list;
        }

        //Get all RecipeBooks where the user is a member
        public List<RecipeBookMember> GetByUser(int userID)
        {
            var list = new List<RecipeBookMember>();

            using var connection = new SQLiteConnection($"Data Source={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM RecipeBookMembers WHERE UserID = @user";
            cmd.Parameters.AddWithValue("@user", userID);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new RecipeBookMember
                {
                    UserID = reader.GetInt32(0),
                    RecipeBookID = reader.GetInt32(1),
                    JoinedDate = DateTime.Parse(reader.GetString(2))
                });
            }

            return list;
        }
    }
}
