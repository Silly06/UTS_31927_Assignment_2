using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

[Route("matches")]
public class MatchesController : Controller
{
    private readonly IMatchService _matchService;

    public MatchesController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpGet("GetMatches")]
    public ActionResult<IEnumerable<Match>> GetMatchesForUser(Guid userId)
    {
        var matches = _matchService.GetMatchesForUser(userId);
        if (matches == null || !matches.Any())
        {
            return NotFound();
        }
        foreach (var match in matches)
        {
            match.Id = new Guid();
        }
        return Ok(matches);
    }

    public void CheckForMatch(Guid postId, Guid currentUser)
    {
        _matchService.CheckForMatch(postId, currentUser);
    }
}
