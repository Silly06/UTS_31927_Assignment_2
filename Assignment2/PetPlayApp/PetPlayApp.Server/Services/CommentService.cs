using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Services.Abstractions;

#nullable enable

namespace PetPlayApp.Server.Services
{
	public class CommentService(IRepositoryProviderService repositoryProvider, INotificationService notificationService)
		: ICommentService
	{
		private readonly IRepository<Comment> _commentRepository = repositoryProvider.GetRepository<Comment>();
		private readonly IRepository<Post> _postRepository = repositoryProvider.GetRepository<Post>();
		private readonly IRepository<User> _userRepository = repositoryProvider.GetRepository<User>();

		public void AddComment(Guid postId, Guid userId, string? content)
		{
			var post = _postRepository.GetById(postId) ?? throw new ArgumentException("Post not found");
			var user = _userRepository.GetById(userId) ?? throw new ArgumentException("User not found");
			var creator = _userRepository.GetById(post.PostCreatorId) ?? throw new ArgumentException("Post creator not found");
			if (string.IsNullOrWhiteSpace(content))
			{
				throw new ArgumentException("Content was empty");
			}

			var comment = new Comment
			{
				Id = Guid.NewGuid(),
				PostId = postId,
				Post = post,
				UserId = userId,
				User = user,
				Content = content,
				CreatedAt = DateTime.UtcNow,
				Likes = []
			};
			notificationService.NotifyCommentCreated(postId, userId, post.PostCreatorId);
			_commentRepository.Add(comment);
		}

		public List<CommentDto> GetCommentsForPost(Guid postId)
		{
			return _commentRepository
				.GetAll()
				.Where(c => c.PostId == postId)
				.OrderByDescending(c => c.CreatedAt)
				.Select(x => 
					new CommentDto
					{
						Content = x.Content,
						CreatedAt = x.CreatedAt,
						UserName = (_userRepository.GetById(x.UserId) ?? throw new InvalidDataException()).UserName ?? string.Empty
					})
				.ToList();
		}

		public void LikeComment(Guid commentId, Guid userId)
		{
			var comment = _commentRepository.GetById(commentId) ?? throw new ArgumentException("Post not found");
			var user = _userRepository.GetById(userId) ?? throw new ArgumentException("User not found");
			notificationService.NotifyCommentLiked(comment.PostId, userId, comment.UserId);
			comment.Likes.Add(user);
			_commentRepository.Update(comment);
		}

		public void UnlikeComment(Guid commentId, Guid userId)
		{
			var comment = _commentRepository.GetById(commentId) ?? throw new ArgumentException("Post not found");
			var user = _userRepository.GetById(userId) ?? throw new ArgumentException("User not found");
			comment.Likes.Remove(user);
			_commentRepository.Update(comment);
		}
	}
}
