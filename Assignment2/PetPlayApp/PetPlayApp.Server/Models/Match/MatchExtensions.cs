namespace PetPlayApp.Server.Models.Match
{
    public static class MatchExtensions
    {
        public static bool CheckUserResponse(this Match match, int user, UserResponse response)
        {
            if (user == 1)
            {
                return match.User1Response == (int)response;
            }
            else if (user == 2)
            {
                return match.User2Response == (int)response;
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
    }
}
