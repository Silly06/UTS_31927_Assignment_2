using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Controllers;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Test
{
	[TestFixture]
	public class NotificationControllerTests
	{
		private Mock<INotificationService> _notificationServiceMock;
		private NotificationController _controller;

		[SetUp]
		public void Setup()
		{
			_notificationServiceMock = new Mock<INotificationService>();
			_controller = new NotificationController(_notificationServiceMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_controller.Dispose();
		}

		[Test]
		public void GetNotificationDetails_ValidPostId_ReturnsOk()
		{
			var id = Guid.NewGuid();
			var notification = new Notification { Id = id };
			_notificationServiceMock.Setup(s => s.GetNotification(id)).Returns(notification);

			var result = _controller.GetNotificationDetails(id) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo(notification));
			});
		}
	}
}
