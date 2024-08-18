using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions
{
	public interface INotificationService
	{
		void NotifyCommentCreated(Guid postId, Guid userId);
		void NotifyPostLiked(Guid postId, Guid userId);
		void NotifyCommentLiked(Guid postId, Guid userId);
		List<Guid> GetRecentNotifications(Guid userId);
		public Notification? GetNotification(Guid notificationId);
	}
}
