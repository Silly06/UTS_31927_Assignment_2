using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions
{
    public interface INotificationService
    {
        void NotifyCommentCreated(Guid postId, Guid userId, string content);
		List<Notification> GetNotificationsForUser(Guid userId);

	}
}
