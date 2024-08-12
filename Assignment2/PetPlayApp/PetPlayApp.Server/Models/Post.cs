namespace PetPlayApp.Server.Models
{
    public class Post
    {
        public Guid PK { get; set; }

        public DateTime DateTimePosted { get; set; }

        public Guid? PostCreatorId { get; set; }

        public User? PostCreator { get; set; }

        public string? Description { get; set; }

        public List<User>? Likes { get; set; }
    }
}
