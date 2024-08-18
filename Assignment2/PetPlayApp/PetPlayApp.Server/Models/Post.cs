namespace PetPlayApp.Server.Models;

public class Post
{
	public Guid Id { get; set; }

	public DateTime DateTimePosted { get; set; }

	public User? PostCreator { get; set; }

	public Guid? PostCreatorId { get; set; }

	public string? Description { get; set; }

	public List<User> Likes { get; init; } = [];

	public ICollection<Comment> Comments { get; init; } = [];

	public byte[]? ImageData { get; set; } // Property to store image data
}
