#nullable enable

namespace PetPlayApp.Server.Dto;

public class CreateUserDto
{
	public string? Username { get; set; }

	public string? Email { get; set; }

	public string? Password { get; set; }

	public int? Age { get; set; }

	public string? Bio { get; set; }
}