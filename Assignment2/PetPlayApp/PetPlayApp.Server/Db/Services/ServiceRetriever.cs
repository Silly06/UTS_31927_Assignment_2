using PetPlayApp.Server.Db.Repos;

namespace PetPlayApp.Server.Db.Services
{
    public static class ServiceRetriever
    {
        private static UserService users;
        private static MatchService matches;
        private static PostService posts;

        public static UserService UserService { get { return users; } set { users = value; } }
        public static MatchService MatchService { get { return matches; } set { matches = value; } }
        public static PostService PostService { get { return posts; } set { posts = value; } }
    }
}
// I feel like I have made some very unnecessary shit here that could be done a better way, but until I figure out a better way this is staying