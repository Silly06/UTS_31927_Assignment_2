// File: PetPlayApp/PetPlayApp.Server/Services/Abstractions/ICommentService.cs
using PetPlayApp.Server.Dto;

namespace PetPlayApp.Server.Services.Abstractions
{
	public interface ICommentService
	{
		void AddComment(Guid postId, Guid userId, string? content);
		List<CommentDto> GetCommentsForPost(Guid postId);
		void LikeComment(Guid commentId, Guid userId);
		void UnlikeComment(Guid commentId, Guid userId);
	}
}
