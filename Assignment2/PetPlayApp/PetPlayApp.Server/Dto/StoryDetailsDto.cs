#nullable enable
namespace PetPlayApp.Server.Dto;

public class StoryDetailsDto
{
	public Guid? StoryId { get; set; }

	public Guid? StoryCreatorId { get; set; }

	public string? StoryCreatorName { get; set; }

	public byte[]? ImageData { get; set; }

	public DateTime? DateTimePosted { get; set; }
}