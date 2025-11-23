using RecipeBookAPI.Models;
using System.Data.SQLite;

namespace RecipeBookAPI.Repositories
{
    public class UserRepository
    {
        private const string DbFile = "recipeBook.db";

        public UserRepository()
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL,
                PasswordHash TEXT NOT NULL,
                DateRegistered TEXT NOT NULL,
                ProfilePicURL TEXT,
                Bio TEXT
            );";
            cmd.ExecuteNonQuery();
        }
        public void Insert(User u)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO Users
                (Name, Email, PasswordHash, DateRegistered, ProfilePicURL, Bio)
            VALUES
                (@name, @email, @hash, @date, @img, @bio)";
            cmd.Parameters.AddWithValue("@name", u.Name);
            cmd.Parameters.AddWithValue("@email", u.Email);
            cmd.Parameters.AddWithValue("@hash", u.PasswordHash);
            cmd.Parameters.AddWithValue("@date", u.DateRegistered.ToString("o"));
            cmd.Parameters.AddWithValue("@img", u.ProfilePicURL);
            cmd.Parameters.AddWithValue("@bio", u.Bio);

            cmd.ExecuteNonQuery();
        }

        public void Update(User u)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
            UPDATE Users
            SET
                Name = @name,
                Email = @email,
                PasswordHash = @hash,
                DateRegistered = @date,
                ProfilePicURL = @img,
                Bio = @bio
            WHERE
                UserID = @id";
            cmd.Parameters.AddWithValue("@name", u.Name);
            cmd.Parameters.AddWithValue("@email", u.Email);
            cmd.Parameters.AddWithValue("@hash", u.PasswordHash);
            cmd.Parameters.AddWithValue("@date", u.DateRegistered.ToString("o"));
            cmd.Parameters.AddWithValue("@img", u.ProfilePicURL);
            cmd.Parameters.AddWithValue("@bio", u.Bio);
            cmd.Parameters.AddWithValue("@id", u.UserID);

            cmd.ExecuteNonQuery();
        }

        public User? GetByID(int id)
        {
            using var connection = new SQLiteConnection($"Data Source ={DbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE UserID = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHash = reader.GetString(3),
                    DateRegistered = DateTime.Parse(reader.GetString(4)),
                    ProfilePicURL = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Bio = reader.IsDBNull(6) ? null : reader.GetString(6)
                };
            }
            return null;
        }
    }
}
