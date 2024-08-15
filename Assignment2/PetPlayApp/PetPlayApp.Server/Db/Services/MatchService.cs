using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Extensions;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Db.Services
{
    public class MatchService
    {
        private readonly Repository<Match> matchRepository;
        private readonly UserService userService;

        public MatchService(UserService userService, RepositoryProvider repositoryProvider)
        {
			matchRepository = repositoryProvider.GetRepository<Match>();
			this.userService = userService;
		}


        public void UpdateMatchStatus(Match match)
        {
            if (match.CheckUserResponse(1, UserResponse.Accepted) && match.CheckUserResponse(2, UserResponse.Accepted))
            {
                match.OverallStatus = (int)MatchStatus.Success;
                ConfirmMatch(match);
            }
            else if (match.IsAwaitingResponse())
            {
                match.OverallStatus = (int)MatchStatus.AwaitingResponse;
            }
            else if (match.IsMatchRejected())
            {
                match.OverallStatus = (int)MatchStatus.Failure;
            }
            matchRepository.Update(match);
        }

        public List<Match> GetAllMatches()
        {
            return matchRepository.GetAll().ToList();
        }

        public Match? GetMatch(Guid id)
        {
            return matchRepository.GetById(id);
        }

        public void AddMatch(User user1, User user2, int user1Response = (int) UserResponse.Pending, int user2Response = (int)UserResponse.Pending, int overallStatus = (int)MatchStatus.AwaitingResponse)
        {

            if (ValidateMatch(user1, user2, user1Response, user2Response, overallStatus))
            {
                matchRepository.Add(new Match { User1 = user1, User2 = user2, User1Response = user1Response, User2Response = user2Response, OverallStatus = overallStatus });
            }
            else
            {
                // validation error
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
                if(match.User1 == null || match.User2 == null)
                {
                    throw new NullReferenceException();
				}
                match.User1.UserStatus = (int)UserStatus.Matched;
                match.User2.UserStatus = (int)UserStatus.Matched;
            }
        }

        public void SeedMatches()
        {
            matchRepository.RemoveAll();

            List<User> users = userService.GetAllUsers().ToList();

            AddMatch(users[0], users[1]); // Pending match between Todd and Baldwin
            AddMatch(users[2], users[3], (int)UserResponse.Accepted, (int)UserResponse.Accepted, (int)MatchStatus.Success); //Accepted match between Aidan and Garfield
            AddMatch(users[4], users[0], (int)UserResponse.Accepted, (int)UserResponse.Rejected, (int)MatchStatus.Failure); // Failed match between Danny and Todd
        }
    }
}
