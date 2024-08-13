namespace PetPlayApp.Server.Models
{
    public class Match
    {
        public User User1 { get; set; }
        public User User2 { get; set; }
        public int User1Response { get; set; }
        public int User2Response { get; set; }
        public int OverallStatus { get; set; }

        public Match(User user1, User user2, int user1Response, int user2Response, int overallStatus)
        {
            User1 = user1;
            User2 = user2;
            User1Response = user1Response;
            User2Response = user2Response;
            OverallStatus = overallStatus;
        }
    }

    public enum MatchStatus
    {
        Success,
        Failure,
        AwaitingResponse
    }

    public enum UserResponse
    {
        Accepted,
        Rejected,
        Pending
    }
}