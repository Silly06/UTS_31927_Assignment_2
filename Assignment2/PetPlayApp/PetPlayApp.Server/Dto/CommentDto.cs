namespace PetPlayApp.Server.Dto
{
	public class CommentDto
	{
		public string? Content { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public string? UserName { get; set; }
	}
}
