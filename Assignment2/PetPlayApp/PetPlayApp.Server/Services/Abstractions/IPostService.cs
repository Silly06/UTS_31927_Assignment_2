using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions
{
    public interface IPostService
    {
        void LikePost(Guid postId, Guid userId);

        void UnlikePost(Guid postId, Guid userId);
        
        List<Guid> GetRecentPosts(int page);
        List<Guid> GetUserPosts(int page, Guid userId);
        Post? GetPost(Guid postId);
        void AddPost(Post post);
    }
}
