using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Extensions
{
	public static class MatchExtensions
	{
		public static bool CheckUserResponse(this Match match, int user, UserResponse response)
		{
			if (user == 1)
			{
				return match.User1Response == response;
			}
			else if (user == 2)
			{
				return match.User2Response == response;
			}
			return false;
		}

		public static bool IsAwaitingResponse(this Match match)
		{
			return match.CheckUserResponse(1, UserResponse.Pending) || match.CheckUserResponse(2, UserResponse.Pending);
		}

		public static bool IsMatchRejected(this Match match)
		{
			return match.CheckUserResponse(1, UserResponse.Rejected) || match.CheckUserResponse(2, UserResponse.Rejected);
		}

		public static bool IsValidResponse(this Match match, int user) // will be used when updating match status
		{
			return match.CheckUserResponse(user, UserResponse.Accepted) | match.CheckUserResponse(user, UserResponse.Rejected) | match.CheckUserResponse(user, UserResponse.Pending);
		}
	}
}
