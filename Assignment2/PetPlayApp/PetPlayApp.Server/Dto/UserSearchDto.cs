#nullable enable
namespace PetPlayApp.Server.Dto;

public class UserSearchDto
{
	public Guid? UserId { get; init; }

	public string? Username { get; init; }

	public byte[]? ProfilePicture { get; init; }
}