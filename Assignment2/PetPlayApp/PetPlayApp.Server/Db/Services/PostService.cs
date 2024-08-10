using PetPlayApp.Server.Db.Repos;

namespace PetPlayApp.Server.Db.Services
{
    public class PostService
    {
        private readonly PostRepository _postRepo;

        public PostService(PostRepository postRepo)
        {
            _postRepo = postRepo;
        }
    }
}
