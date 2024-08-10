using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models.Match;

namespace PetPlayApp.Server.Db.Services
{
    public class MatchService
    {
        private readonly MatchRepository _matchRepo;

        public MatchService(MatchRepository matchRepo)
        {
            _matchRepo = matchRepo;
        }


        public void UpdateMatchStatus(Match match)
        {
            if (match.CheckUserResponse(1, UserResponse.Accepted) && match.CheckUserResponse(2, UserResponse.Accepted))
            {
                match.OverallStatus = (int)MatchStatus.Success;
            }
            else if (match.IsAwaitingResponse())
            {
                match.OverallStatus = (int)MatchStatus.AwaitingResponse;
            }
            else if (match.IsMatchRejected())
            {
                match.OverallStatus = (int)MatchStatus.Failure;
            }
        }

        public List<Match> GetAllMatches()
        {
            return _matchRepo.GetAll().ToList();
        }
    }
}
