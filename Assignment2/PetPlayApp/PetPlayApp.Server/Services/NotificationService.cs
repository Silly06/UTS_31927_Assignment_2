using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services

{
	public class NotificationService : INotificationService
	{
		private readonly IRepository<Notification> notificationRepository;
		public NotificationService(IRepositoryProviderService repositoryProviderService)
		{
			notificationRepository = repositoryProviderService.GetRepository<Notification>();
		}



		public void NotifyCommentCreated(Guid postId, Guid userId)
		{
			notificationRepository.Add(new Notification
			{
				Id = Guid.NewGuid(),
				SubjectId = userId,
				Timestamp = DateTime.Now,
				NotificationType = NotificationType.Comment,
				PostId = postId
			});
		}

		public void NotifyCommentLiked(Guid postId, Guid userId)
		{
			notificationRepository.Add(new Notification
			{
				Id = Guid.NewGuid(),
				SubjectId = userId,
				Timestamp = DateTime.Now,
				NotificationType = NotificationType.CommentLike,
				PostId = postId
			});
		}

		public void NotifyPostLiked(Guid postId, Guid userId)
		{
			notificationRepository.Add(new Notification
			{
				Id = Guid.NewGuid(),
				SubjectId = userId,
				Timestamp = DateTime.Now,
				NotificationType = NotificationType.PostLike,
				PostId = postId
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
