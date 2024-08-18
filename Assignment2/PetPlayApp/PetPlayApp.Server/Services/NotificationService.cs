using Microsoft.Extensions.Hosting;
using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services

{
	public class NotificationService : INotificationService
	{
		private readonly IRepository<Notification> notificationRepository;
		private readonly IRepository<Post> postRepository;
		private readonly IRepository<User> userRepository;


		public NotificationService(IRepositoryProviderService repositoryProviderService)
		{
			notificationRepository = repositoryProviderService.GetRepository<Notification>();
			postRepository = repositoryProviderService.GetRepository<Post>();
			userRepository = repositoryProviderService.GetRepository<User>();
		}



		public void NotifyCommentCreated(Guid postId, Guid userId)
		{
			var post = postRepository.GetById(postId);
			var user = userRepository.GetById(userId);
			notificationRepository.Add(new Notification
			{
				Id = Guid.NewGuid(),
				SubjectId = userId,
				Timestamp = DateTime.Now,
				NotificationType = NotificationType.Comment,
				PostId = postId,
				Post = post,
				Subject = user
			});
		}

		public void NotifyCommentLiked(Guid postId, Guid userId)
		{
			var post = postRepository.GetById(postId);
			var user = userRepository.GetById(userId);
			notificationRepository.Add(new Notification
			{
				Id = Guid.NewGuid(),
				SubjectId = userId,
				Timestamp = DateTime.Now,
				NotificationType = NotificationType.CommentLike,
				PostId = postId,
				Post = post,
				Subject = user
			});
		}

		public void NotifyPostLiked(Guid postId, Guid userId)
		{
			var post = postRepository.GetById(postId);
			var user = userRepository.GetById(userId);
			notificationRepository.Add(new Notification
			{
				Id = Guid.NewGuid(),
				SubjectId = userId,
				Timestamp = DateTime.Now,
				NotificationType = NotificationType.PostLike,
				PostId = postId,
				Post = post,
				Subject = user
			});
		}

		public List<Guid> GetRecentNotifications(Guid userId)
		{
			return notificationRepository.GetAll()
				.Where(n => n.SubjectId == userId)
				.OrderBy(x => x.Timestamp)
				.Select(x => x.Id)
				.ToList();
		}

		public Notification? GetNotification(Guid notificationId)
		{
			var notification = notificationRepository.GetById(notificationId);
			return notification;
		}
	}
}
