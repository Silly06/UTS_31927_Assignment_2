// File: PetPlayApp/PetPlayApp.Server/Services/Abstractions/ICommentService.cs
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions
{
    public interface ICommentService
    {
        Comment AddComment(Guid postId, Guid userId, string? content);
        List<Comment> GetCommentsForPost(Guid postId);
	}
}
