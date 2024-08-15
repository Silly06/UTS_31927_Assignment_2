using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Db.Services
{
    public interface IPostService
    {
        List<Guid> GetRecentPosts(int page);
        List<Guid> GetUserPosts(int page, Guid userId);
        Post? GetPost(Guid postId);
        void AddPost(Post post);
    }
}
