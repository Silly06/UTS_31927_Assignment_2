namespace PetPlayApp.Server
{
	public class Post
	{
		public DateTime DateTimePosted { get; set; }

		public User PostCreator { get; set; }

		public string? Description { get; set; }
	}
}
