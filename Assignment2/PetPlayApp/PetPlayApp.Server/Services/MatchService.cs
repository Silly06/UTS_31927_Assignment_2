using PetPlayApp.Server.Db;
using PetPlayApp.Server.Extensions;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;


namespace PetPlayApp.Server.Services
{
    public class MatchService : IMatchService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IUserService userService;

        public MatchService(IUserService userService, IRepositoryProviderService repositoryProvider)
        {
			matchRepository = repositoryProvider.GetRepository<Match>();
			this.userService = userService;
		}


        public void UpdateMatchStatus(Match match)
        {
            if (match.CheckUserResponse(1, UserResponse.Accepted) && match.CheckUserResponse(2, UserResponse.Accepted))
            {
                match.OverallStatus = MatchStatus.Success;
                ConfirmMatch(match);
            }
            else if (match.IsAwaitingResponse())
            {
                match.OverallStatus = MatchStatus.AwaitingResponse;
            }
            else if (match.IsMatchRejected())
            {
                match.OverallStatus = MatchStatus.Failure;
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

        public void AddMatch(User user1, User user2, UserResponse user1Response = UserResponse.Pending, UserResponse user2Response = UserResponse.Pending, MatchStatus overallStatus = MatchStatus.AwaitingResponse)
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

        private bool ValidateMatch(User user1, User user2, UserResponse user1Response = UserResponse.Pending, UserResponse user2Response = UserResponse.Pending, MatchStatus overallStatus = MatchStatus.AwaitingResponse)
        {
            var validMatch = true;
            if (user1 == null || user2 == null)
            {
                validMatch = false;
            }
            if (!(user1Response == UserResponse.Accepted || user1Response == UserResponse.Rejected || user1Response == UserResponse.Pending)
                || !(user2Response == UserResponse.Accepted || user2Response == UserResponse.Rejected || user2Response == UserResponse.Pending))
            {
                validMatch = false;
            }
            if (!(overallStatus == MatchStatus.Success || overallStatus == MatchStatus.Failure || overallStatus == MatchStatus.AwaitingResponse))
            {
                validMatch = false;
            }
            return validMatch;
        }

        public void ConfirmMatch(Match match)
        {
            if (match.OverallStatus == MatchStatus.Success)
            {
                if(match.User1 == null || match.User2 == null)
                {
                    throw new NullReferenceException();
				}
                match.User1.UserStatus = UserStatus.Matched;
                match.User2.UserStatus = UserStatus.Matched;
            }
        }
    }
}
