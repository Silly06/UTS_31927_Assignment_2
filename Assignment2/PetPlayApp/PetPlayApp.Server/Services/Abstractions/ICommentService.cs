// File: PetPlayApp/PetPlayApp.Server/Services/Abstractions/ICommentService.cs
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions
{
	public interface ICommentService
	{
		void AddComment(Guid postId, Guid userId, string? content);
		List<Comment> GetCommentsForPost(Guid postId);
		void LikeComment(Guid commentId, Guid userId);
		void UnlikeComment(Guid commentId, Guid userId);
	}
}
