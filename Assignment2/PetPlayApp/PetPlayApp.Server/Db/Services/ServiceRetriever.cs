using PetPlayApp.Server.Db.Repos;

namespace PetPlayApp.Server.Db.Services
{
    public class ServiceRetriever
    {
        private UserRepository _userRepository;
        private MatchRepository _matchRepository;
        private PostRepository _postRepository;

        public ServiceRetriever(UserRepository userRepository, MatchRepository matchRepository, PostRepository postRepository)
        {
            _userRepository = userRepository;
            _matchRepository = matchRepository;
            _postRepository = postRepository;
        }

        public UserRepository UserRepo { get { return _userRepository; } }
        public MatchRepository MatchRepo { get { return _matchRepository; } }
        public PostRepository PostRepo { get { return _postRepository; } }
    }
}
// I feel like I have made some very unnecessary shit here that could be done a better way, but until I figure out a better way this is staying