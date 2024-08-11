namespace PetPlayApp.Server.Models
{
    public class Post
    {
        public DateTime DateTimePosted { get; set; }

        public User PostCreator { get; set; }

        public string? Description { get; set; }

        public List<User>? Likes { get; set; }
    }
}
// :)