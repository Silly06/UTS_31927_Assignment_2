using Moq;
using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Controllers;
using PetPlayApp.Server.Services.Abstractions;
using PetPlayApp.Server.Models;


namespace PetPlayApp.Test.Controllers
{
	[TestFixture]
	public class CommentControllerTests
	{
		private Mock<ICommentService> _commentServiceMock;
		private CommentController _controller;

		[SetUp]
		public void Setup()
		{
			_commentServiceMock = new Mock<ICommentService>();
			_controller = new CommentController(_commentServiceMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_controller.Dispose();
		}

		[Test]
		public void AddComment_ValidData_ReturnsOk()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var content = "Test comment";

			var result = _controller.AddComment(postId, userId, content) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo("Comment created successfully."));
			});
		}

		[Test]
		public void AddComment_InvalidData_ReturnsBadRequest()
		{
			var result = _controller.AddComment(Guid.Empty, Guid.Empty, null) as BadRequestObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(400));
				Assert.That(result.Value, Is.EqualTo("Invalid request data."));
			});
		}

		[Test]
		public void GetComments_ValidRequest_ReturnsOk()
		{
			var request = new GetCommentRequest { PostId = Guid.NewGuid() };
			_commentServiceMock.Setup(s => s.GetCommentsForPost(request.PostId)).Returns(new List<Comment>());

			var result = _controller.GetComments(request) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo(200));
		}

		[Test]
		public void LikeComment_ValidData_ReturnsOk()
		{
			var commentId = Guid.NewGuid();
			var userId = Guid.NewGuid();

			var result = _controller.LikePost(commentId, userId) as OkResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo(200));
		}

		[Test]
		public void UnlikeComment_ValidData_ReturnsOk()
		{
			var commentId = Guid.NewGuid();
			var userId = Guid.NewGuid();

			var result = _controller.UnlikePost(commentId, userId) as OkResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo(200));
		}
	}
}
