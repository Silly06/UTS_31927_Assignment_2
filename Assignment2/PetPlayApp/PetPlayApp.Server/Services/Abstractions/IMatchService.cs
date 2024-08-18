// File: PetPlayApp/PetPlayApp.Server/Services/Abstractions/IMatchService.cs
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions
{
	public interface IMatchService
	{
		public void UpdateMatchStatus(Match match);


		public List<Match> GetAllMatches();
		public void AddMatch(User user1, User user2, UserResponse user1Response = UserResponse.Pending, UserResponse user2Response = UserResponse.Pending, MatchStatus overallStatus = MatchStatus.AwaitingResponse);
		public void ConfirmMatch(Match match);
		public IEnumerable<Match> GetMatchesForUser(Guid id);
        void CheckForMatch(Guid postId, Guid currentUser);
    }
}
