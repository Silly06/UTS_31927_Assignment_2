using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Dto;

public class MatchDetailsDto
{
	public Guid? MatchId { get; set; }

	public string? User1Name { get; set; }
	public string? User2Name { get; set; }
	public MatchStatus? MatchStatus { get; set; }
	public UserResponse? Response1 { get; set; }
	public UserResponse? Response2 { get; set; }
}