using RecipeBookAPI.Repositories;
using RecipeBookAPI.Models;

namespace RecipeBookAPI.Services
{
    public class UserService
    {
        public readonly UserRepository _repo = new UserRepository();

        public void RegisterUser(string name, string email, string password)
        {
            if (GetUserByEmail(email) == null)
            {
                User newUser = new User
                {
                    Name = name,
                    Email = email,
                    //how to hash password? PasswordHash = HashPassword(password)
                    PasswordHash = password,
                    DateRegistered = DateTime.UtcNow,

                    //REVIEW
                    ProfilePicURL = "profile",
                    Bio = "null"
                };

                _repo.Insert(newUser);
            }
            else
            {
                throw new Exception("Email is being used already");
            }
        }
        //To review
        public void UpdateUser(int userID, User updatedData)
        {

        }

        public User? GetUser(int id)
        {
            User? user = _repo.GetByID(id);

            return user;
        }

        public User? GetUserByEmail(string email)
        {
            User? user = _repo.GetByEmail(email);

            return user;
        }
    }
}
