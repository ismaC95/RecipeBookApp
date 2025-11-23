namespace RecipeBookAPI.Models
{
    public class User
    {
        public int UserID { get; set; }
        required public string Name { get; set; }
        required public string Email { get; set; }
        required public string PasswordHash { get; set; }
        public DateTime DateRegistered { get; set; }
        public string? ProfilePicURL { get; set; }
        public string? Bio { get; set; }

    }
}
