namespace PetPlayApp.Server.Models.Match
{
    public class Match
    {
        public User User1 { get; set; }
        public User User2 { get; set; }
        public int User1Response { get; set; }
        public int User2Response { get; set; }
        public int OverallStatus { get; set; }
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