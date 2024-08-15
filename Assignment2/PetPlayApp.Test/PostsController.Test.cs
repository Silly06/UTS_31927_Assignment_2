using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PetPlayApp.Server.Controllers;
using PetPlayApp.Server.Db.Services;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Test
{
	public class PostsControllerTests
	{
		private Mock<IPostService> mockPostService;
		private PostsController postsController;

		[SetUp]
		public void Setup()
		{
			mockPostService = new Mock<IPostService>();
			postsController = new PostsController(mockPostService.Object);
		}

		[TearDown]
		public void TearDown()
		{
			postsController.Dispose();
		}

		[Test]
		public void GetRecentPosts_ReturnsOkResult_WithListOfPostIds()
		{
			// Arrange
			var postIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
			mockPostService.Setup(service => service.GetRecentPosts(It.IsAny<int>())).Returns(postIds);

			// Act
			var result = postsController.GetRecentPosts(1) as OkObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo(postIds));
			});
		}

		[Test]
		public void GetUserPosts_ReturnsOkResult_WithListOfPostIds()
		{
			// Arrange
			var postIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
			mockPostService.Setup(service => service.GetUserPosts(It.IsAny<int>(), It.IsAny<Guid>())).Returns(postIds);

			// Act
			var result = postsController.GetUserPosts(1, Guid.NewGuid()) as OkObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo(postIds));
			});
		}

		[Test]
		public void GetPostDetails_ReturnsOkResult_WithPost()
		{
			// Arrange
			var post = new Post { Id = Guid.NewGuid(), Description = "Test Post" };
			mockPostService.Setup(service => service.GetPost(It.IsAny<Guid>())).Returns(post);

			// Act
			var result = postsController.GetPostDetails(post.Id) as OkObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo(post));
			});
		}

		[Test]
		public async Task NewPost_ReturnsOkResult_WithCreatedPost()
		{
			// Arrange
			var postCreatorId = Guid.NewGuid();
			var description = "Test Description";
			var image = new FormFile(new MemoryStream([]), 0, 1, "Data", "dummy.png");
			mockPostService.Setup(service => service.AddPost(It.IsAny<Post>())).Verifiable();

			// Act
			var result = await postsController.NewPost(image, description, postCreatorId) as OkObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.InstanceOf<Post>());
			});
			mockPostService.Verify(service => service.AddPost(It.IsAny<Post>()), Times.Once);
		}

		[Test]
		public async Task NewPost_ReturnsBadRequest_WhenImageIsNull()
		{
			// Arrange
			var postCreatorId = Guid.NewGuid();
			var description = "Test Description";

			// Act
			var result = await postsController.NewPost(null, description, postCreatorId) as BadRequestObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(400));
				Assert.That(result.Value, Is.EqualTo("Image is required"));
			});
		}
	}
}