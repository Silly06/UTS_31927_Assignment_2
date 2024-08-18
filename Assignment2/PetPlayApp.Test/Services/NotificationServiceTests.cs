using Moq;
using NUnit.Framework;
using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services;
using PetPlayApp.Server.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetPlayApp.Test.Services
{
	[TestFixture]
	public class NotificationServiceTests
	{
		private Mock<IRepositoryProviderService> _repositoryProviderMock;
		private Mock<IRepository<Notification>> _notificationRepositoryMock;
		private Mock<IRepository<Post>> _postRepositoryMock;
		private Mock<IRepository<User>> _userRepositoryMock;
		private NotificationService _notificationService;

		[SetUp]
		public void Setup()
		{
			_repositoryProviderMock = new Mock<IRepositoryProviderService>();
			_notificationRepositoryMock = new Mock<IRepository<Notification>>();
			_postRepositoryMock = new Mock<IRepository<Post>>();
			_userRepositoryMock = new Mock<IRepository<User>>();

			_repositoryProviderMock.Setup(r => r.GetRepository<Notification>()).Returns(_notificationRepositoryMock.Object);
			_repositoryProviderMock.Setup(r => r.GetRepository<Post>()).Returns(_postRepositoryMock.Object);
			_repositoryProviderMock.Setup(r => r.GetRepository<User>()).Returns(_userRepositoryMock.Object);

			_notificationService = new NotificationService(_repositoryProviderMock.Object);
		}

		[Test]
		public void NotifyCommentCreated_ValidData_AddsNotification()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var post = new Post { Id = postId };
			var user = new User { Id = userId };

			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
			_userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

			_notificationService.NotifyCommentCreated(postId, userId);

			_notificationRepositoryMock.Verify(r => r.Add(It.Is<Notification>(n =>
				n.SubjectId == userId &&
				n.PostId == postId &&
				n.NotificationType == NotificationType.Comment &&
				n.Subject == user &&
				n.Post == post
			)), Times.Once);
		}

		[Test]
		public void NotifyCommentLiked_ValidData_AddsNotification()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var post = new Post { Id = postId };
			var user = new User { Id = userId };

			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
			_userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

			_notificationService.NotifyCommentLiked(postId, userId);

			_notificationRepositoryMock.Verify(r => r.Add(It.Is<Notification>(n =>
				n.SubjectId == userId &&
				n.PostId == postId &&
				n.NotificationType == NotificationType.CommentLike &&
				n.Subject == user &&
				n.Post == post
			)), Times.Once);
		}

		[Test]
		public void NotifyPostLiked_ValidData_AddsNotification()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var post = new Post { Id = postId };
			var user = new User { Id = userId };

			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
			_userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

			_notificationService.NotifyPostLiked(postId, userId);

			_notificationRepositoryMock.Verify(r => r.Add(It.Is<Notification>(n =>
				n.SubjectId == userId &&
				n.PostId == postId &&
				n.NotificationType == NotificationType.PostLike &&
				n.Subject == user &&
				n.Post == post
			)), Times.Once);
		}

		[Test]
		public void GetRecentNotifications_ValidUserId_ReturnsNotifications()
		{
			var userId = Guid.NewGuid();
			var notifications = new List<Notification>
			{
				new Notification { Id = Guid.NewGuid(), SubjectId = userId, Timestamp = DateTime.UtcNow.AddMinutes(-1) },
				new Notification { Id = Guid.NewGuid(), SubjectId = userId, Timestamp = DateTime.UtcNow.AddMinutes(-2) }
			};

			_notificationRepositoryMock.Setup(r => r.GetAll()).Returns(notifications.AsQueryable());

			var result = _notificationService.GetRecentNotifications(userId);

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public void GetNotification_ValidNotificationId_ReturnsNotification()
		{
			var notificationId = Guid.NewGuid();
			var notification = new Notification { Id = notificationId };

			_notificationRepositoryMock.Setup(r => r.GetById(notificationId)).Returns(notification);

			var result = _notificationService.GetNotification(notificationId);

			Assert.That(result, Is.EqualTo(notification));
		}

		[Test]
		public void GetNotification_InvalidNotificationId_ReturnsNull()
		{
			var notificationId = Guid.NewGuid();

#pragma warning disable CS8600 // Testing null for nullable type
			_notificationRepositoryMock.Setup(r => r.GetById(notificationId)).Returns((Notification)null);
#pragma warning restore CS8600 // Testing null for nullable type

			var result = _notificationService.GetNotification(notificationId);

			Assert.That(result, Is.Null);
		}
	}
}
