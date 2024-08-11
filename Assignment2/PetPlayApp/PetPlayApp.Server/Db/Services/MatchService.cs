using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Extensions;
using PetPlayApp.Server.Models;

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

        public Match GetMatch(int id)
        {
            return _matchRepo.GetById(id);
        }

        public void AddMatch(Match match)
        {
            if (ValidateMatch(match))
            _matchRepo.Add(match);
        }

        private bool ValidateMatch(Match match)
        {
            return !match.User1.IsMatched() && !match.User2.IsMatched();
        }

        public void ConfirmMatch(Match match)
        {
            if (match.OverallStatus == (int)MatchStatus.Success)
            {
                match.User1.UserStatus = (int)UserStatus.Matched;
                match.User2.UserStatus = (int)UserStatus.Matched;
            }
        }
    }
}
