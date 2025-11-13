namespace RecipeBookAPI.Models
{
    public class User
    {
        public int userID { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public DateTime dateRegistered { get; set; }
        public string? profilePicURL { get; set; }
        public string? bio { get; set; }

    }
}
