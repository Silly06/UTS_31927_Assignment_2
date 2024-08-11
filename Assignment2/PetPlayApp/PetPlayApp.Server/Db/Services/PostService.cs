using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Db.Services
{
    public class PostService
    {
        private readonly PostRepository _postRepo;

        public PostService(PostRepository postRepo)
        {
            _postRepo = postRepo;
        }

        public void LikePost(Post post, User user)
        {
            if (post.Likes == null)
            {
                post.Likes = new List<User>();
            }
            post.Likes.Add(user);
            CheckForMatch(post, user);
        }

        public void CheckForMatch(Post post, User user)
        {
            // if it is matchy match time then make a matchy match
            // also this should be in match service but dw about it
        }
    }
}
