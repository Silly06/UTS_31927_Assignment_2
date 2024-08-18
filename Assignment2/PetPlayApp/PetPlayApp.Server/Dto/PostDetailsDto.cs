namespace PetPlayApp.Server.Dto;

public class PostDetailsDto
{
	public Guid? PostId { get; set; }

	public int? LikesCount { get; set; }

	public bool? LikedByUser { get; set; }

	public string? Description { get; set; }

	public byte[]? ImageData { get; set; }
}