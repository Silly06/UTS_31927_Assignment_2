// File: PetPlayApp/PetPlayApp.Server/Services/Abstractions/INotificationService.cs
namespace PetPlayApp.Server.Services.Abstractions
{
    public interface INotificationService
    {
        void NotifyCommentCreated(Guid postId, Guid userId, string content);
    }
}
