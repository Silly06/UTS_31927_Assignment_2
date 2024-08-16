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



        public void NotifyCommentCreated(Guid postId, Guid userId, string content)
        {
            // Implement your notification logic here
            // For example, send an email, push notification, etc.
            Console.WriteLine($"Notification: A new comment was added to post {postId} by user {userId}. Content: {content}");
        }

		public List<Notification> GetNotificationsForUser(Guid userId)
		{
			return notificationRepository.GetAll().Where(n => n.SubjectId == userId).ToList();
		}
	}
}
