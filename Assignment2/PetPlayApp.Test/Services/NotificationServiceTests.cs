using Moq;
using NUnit.Framework;
using PetPlayApp.Server.Services;
using PetPlayApp.Server.Services.Abstractions;
using System;

namespace PetPlayApp.Test.Services
{
    [TestFixture]
    public class NotificationServiceTests
    {
        private Mock<IRepositoryProviderService> _repositoryProviderMock;
        private NotificationService _notificationService;

        [SetUp]
        public void Setup()
        {
            _repositoryProviderMock = new Mock<IRepositoryProviderService>();
            _notificationService = new NotificationService(_repositoryProviderMock.Object);
        }

        [Test]
        public void NotifyCommentCreated_ValidData_SendsNotification()
        {
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Assuming NotifyCommentCreated sends a notification, we can verify it was called correctly.
            // This is a placeholder as the actual implementation details are not provided.
            _notificationService.NotifyCommentCreated(postId, userId);

            // Verify the notification logic here, e.g., calling a method on a mock.
        }

        [Test]
        public void NotifyCommentLiked_ValidData_SendsNotification()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Assuming NotifyCommentLiked sends a notification, we can verify it was called correctly.
            // This is a placeholder as the actual implementation details are not provided.
            _notificationService.NotifyCommentLiked(commentId, userId);

            // Verify the notification logic here, e.g., calling a method on a mock.
        }
    }
}
