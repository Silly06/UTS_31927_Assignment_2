using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services

{
    public class NotificationService : INotificationService
    {
        public void NotifyCommentCreated(Guid postId, Guid userId, string content)
        {
            // Implement your notification logic here
            // For example, send an email, push notification, etc.
            Console.WriteLine($"Notification: A new comment was added to post {postId} by user {userId}. Content: {content}");
        }
    }
}
