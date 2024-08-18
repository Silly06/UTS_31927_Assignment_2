using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions
{
    public interface IPostService
    {
        PostDetailsDto? LikePost(Guid postId, Guid userId);

        PostDetailsDto? UnlikePost(Guid postId, Guid userId);
        
        List<Guid> GetRecentPosts(int page);
        List<Guid> GetUserPosts(int page, Guid userId);
        Post? GetPost(Guid postId);
        void AddPost(Post post);
    }
}