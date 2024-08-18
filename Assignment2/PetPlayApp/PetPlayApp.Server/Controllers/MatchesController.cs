using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Db;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

[Route("matches")]
public class MatchesController : Controller
{
	private readonly IMatchService _matchService;
	private readonly IRepository<User> _userRepository;

	public MatchesController(IMatchService matchService, IRepositoryProviderService repositoryProviderService)
	{
		_matchService = matchService;
		_userRepository = repositoryProviderService.GetRepository<User>();
	}

	[HttpGet("GetMatches")]
	public IActionResult GetMatchesForUser(Guid userId)
	{
		var matches = _matchService.GetMatchesForUser(userId);
		if (matches == null || !matches.Any())
		{
			return NotFound();
		}
		var matchDetails = new List<MatchDetailsDto>();
		foreach (var match in matches)
		{
			matchDetails.Add(new MatchDetailsDto
			{
				MatchId = new Guid(),
				User1Name = _userRepository.GetById(match.User1Id)?.UserName,
				User2Name = _userRepository.GetById(match.User2Id)?.UserName,
				MatchStatus = match.OverallStatus,
				Response1 = match.User1Response,
				Response2 = match.User2Response
			});
		}
		return Ok(matchDetails);
	}

	[HttpGet("CheckForMatch")]
	public void CheckForMatch([FromBody] LikePostDto likePost)
	{
		_matchService.CheckForMatch(likePost.PostId, likePost.UserId);
	}
}
