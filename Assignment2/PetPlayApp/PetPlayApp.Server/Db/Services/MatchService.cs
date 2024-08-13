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

        public Match GetMatch(Guid id)
        {
            return _matchRepo.GetById(id);
        }

        public void AddMatch(User user1, User user2, int user1Response = (int) UserResponse.Pending, int user2Response = (int)UserResponse.Pending, int overallStatus = (int)MatchStatus.AwaitingResponse)
        {

            if (ValidateMatch(user1, user2, user1Response, user2Response, overallStatus))
            {
                _matchRepo.Add(new Match(user1, user2, user1Response, user2Response, overallStatus));
            }
            else
            {
                //validation error or smt
            }
        }

        private bool ValidateMatch(User user1, User user2, int user1Response = (int)UserResponse.Pending, int user2Response = (int)UserResponse.Pending, int overallStatus = (int)MatchStatus.AwaitingResponse)
        {
            var validMatch = true;
            if (user1 == null || user2 == null)
            {
                validMatch = false;
            }
            if (!(user1Response == (int)UserResponse.Accepted || user1Response == (int)UserResponse.Rejected || user1Response == (int)UserResponse.Pending)
                || !(user2Response == (int)UserResponse.Accepted || user2Response == (int)UserResponse.Rejected || user2Response == (int)UserResponse.Pending))
            {
                validMatch = false;
            }
            if (!(overallStatus == (int)MatchStatus.Success || overallStatus == (int)MatchStatus.Failure || overallStatus == (int)MatchStatus.AwaitingResponse))
            {
                validMatch = false;
            }
            return validMatch;
        }

        public void ConfirmMatch(Match match)
        {
            if (match.OverallStatus == (int)MatchStatus.Success)
            {
                match.User1.UserStatus = (int)UserStatus.Matched;
                match.User2.UserStatus = (int)UserStatus.Matched;
            }
        }

        public void SeedMatches()
        {
            _matchRepo.RemoveAll();

            // need to get access to other services/repos here to seed matches with seeded users but too lazy rn
        }
    }
}
