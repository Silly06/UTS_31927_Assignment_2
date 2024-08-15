// File: PetPlayApp/PetPlayApp.Server/Db/Services/CommentService.cs
using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> commentRepository;
        private readonly IRepository<Post> postRepository;
		private readonly IRepository<User> userRepository;
        private readonly INotificationService notificationService;
		
        public CommentService(IRepositoryProviderService repositoryProvider, INotificationService notificationService)
        {
            commentRepository = repositoryProvider.GetRepository<Comment>();
            postRepository = repositoryProvider.GetRepository<Post>();
            userRepository = repositoryProvider.GetRepository<User>();
			this.notificationService = notificationService;

		}

        public Comment AddComment(Guid postId, Guid userId, string? content)
        {
            var post = postRepository.GetById(postId) ?? throw new ArgumentException("Post not found");
			var user = userRepository.GetById(postId) ?? throw new ArgumentException("User not found");
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
                Content = content
            };
			notificationService.NotifyCommentCreated(postId, userId, content);
			commentRepository.Add(comment);
            return comment;
        }

		public List<Comment> GetCommentsForPost(Guid postId)
		{
			return commentRepository.GetAll().Where(c => c.PostId == postId).ToList();
		}
	}
}
