using RecipeBookAPI.Repositories;
using RecipeBookAPI.Models;

namespace RecipeBookAPI.Services
{
    public class UserServices
    {
        private readonly UserRepository _repo;

        public UserServices(UserRepository repo)
        {
            _repo = repo;
        }

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
                    DateRegistered = DateTime.Now,

                    //REVIEW
                    ProfilePicURL = null,
                    Bio = null,
                };

                _repo.Insert(newUser);
            }
            else
            {
                throw new InvalidOperationException("Email is being used already");
            }
        }
        public void UpdateUser(User updatedUser, int userID)
        {
            var existingUser = _repo.GetByID(userID);

            if (existingUser == null) throw new InvalidOperationException("User doesn't exist");

            updatedUser.UserID = existingUser.UserID;
            updatedUser.DateRegistered = existingUser.DateRegistered;

            _repo.Update(updatedUser);
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

        public User? Login(string email, string password)
        {
            var loggingUser = _repo.GetByEmail(email);
            if(loggingUser == null) throw new InvalidOperationException("Email doesn't exist");

            if(loggingUser.PasswordHash != password) throw new UnauthorizedAccessException("Password is incorrect");

            return loggingUser;
        }
    }
}
