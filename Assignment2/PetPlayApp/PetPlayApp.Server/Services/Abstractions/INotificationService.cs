using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions
{
	public interface INotificationService
	{
		void NotifyCommentCreated(Guid postId, Guid userId, string content);
		List<Guid> GetRecentNotifications(Guid userId);
		public Notification? GetNotification(Guid notificationId);
	}
}
