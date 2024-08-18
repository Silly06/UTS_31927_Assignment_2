using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions
{
	public interface INotificationService
	{
		void NotifyCommentCreated(Guid postId, Guid creatorId, Guid subjectId);
		void NotifyPostLiked(Guid postId, Guid creatorId, Guid subjectId);
		void NotifyCommentLiked(Guid postId, Guid creatorId, Guid subjectId);
		List<Guid> GetRecentNotifications(Guid userId);
		public Notification? GetNotification(Guid notificationId);
	}
}
