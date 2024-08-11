namespace PetPlayApp.Server.Models
{
    public class User
    {
        public Guid PK { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Bio { get; set; }
        public int UserStatus { get; set; }
    }

    public enum UserStatus
    {
        Matched,
        NotMatched
    }
}