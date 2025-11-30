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
                    ProfilePicURL = "profile",
                    Bio = "null"
                };

                _repo.Insert(newUser);
            }
            else
            {
                throw new InvalidOperationException("Email is being used already");
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
