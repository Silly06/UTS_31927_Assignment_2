using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PetPlayApp.Server.Controllers;
using PetPlayApp.Server.Services.Abstractions;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PetPlayApp.Test
{
	[TestFixture]
	public class PostsControllerTests
	{
		private Mock<IPostService> _postServiceMock;
		private PostsController _controller;

		[SetUp]
		public void Setup()
		{
			_postServiceMock = new Mock<IPostService>();
			_controller = new PostsController(_postServiceMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_controller.Dispose();
		}

		[Test]
		public void GetRecentPosts_ValidPage_ReturnsOk()
		{
			var page = 1;
			var postIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
			_postServiceMock.Setup(s => s.GetRecentPosts(page)).Returns(postIds);

			var result = _controller.GetRecentPosts(page) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo(postIds));
			});
		}

		[Test]
		public void GetUserPosts_ValidPageAndUserId_ReturnsOk()
		{
			var page = 1;
			var userId = Guid.NewGuid();
			var postIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
			_postServiceMock.Setup(s => s.GetUserPosts(page, userId)).Returns(postIds);

			var result = _controller.GetUserPosts(page, userId) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo(postIds));
			});
		}

		[Test]
		public void GetPostDetails_ValidPostIdAndUserId_ReturnsOk()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var post = new Post
			{
				Id = postId,
				Likes = new List<User> { new User { Id = userId } },
				ImageData = new byte[] { 1, 2, 3 },
				Description = "Test description"
			};
			_postServiceMock.Setup(s => s.GetPost(postId)).Returns(post);

			var result = _controller.GetPostDetails(postId, userId) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo(200));
			var postDetails = result.Value as PostDetailsDto;
			Assert.That(postDetails, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(postDetails.PostId, Is.EqualTo(postId));
				Assert.That(postDetails.LikesCount, Is.EqualTo(1));
			});
			Assert.IsTrue(postDetails.LikedByUser);
			Assert.Multiple(() =>
			{
				Assert.That(postDetails.ImageData, Is.EqualTo(post.ImageData));
				Assert.That(postDetails.Description, Is.EqualTo(post.Description));
			});
		}

		[Test]
		public async Task NewPost_ValidData_ReturnsOk()
		{
			var postCreatorId = Guid.NewGuid();
			var description = "Test description";
			var fileMock = new Mock<IFormFile>();
			var content = "Fake file content";
			var fileName = "test.png";
			var ms = new MemoryStream();
			var writer = new StreamWriter(ms);
			writer.Write(content);
			writer.Flush();
			ms.Position = 0;
			fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
			fileMock.Setup(_ => _.FileName).Returns(fileName);
			fileMock.Setup(_ => _.Length).Returns(ms.Length);

			var result = await _controller.NewPost(fileMock.Object, description, postCreatorId) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo(200));
			var post = result.Value as Post;
			Assert.That(post, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(post.Description, Is.EqualTo(description));
				Assert.That(post.PostCreatorId, Is.EqualTo(postCreatorId));
			});
		}

		[Test]
		public async Task NewPost_MissingImage_ReturnsBadRequest()
		{
			var postCreatorId = Guid.NewGuid();
			var description = "Test description";

			var result = await _controller.NewPost(null, description, postCreatorId) as BadRequestObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(400));
				Assert.That(result.Value, Is.EqualTo("Image is required"));
			});
		}

		[Test]
		public void LikePost_ErrorProcessingLike_ReturnsBadRequest()
		{
			var likePostDto = new LikePostDto { PostId = Guid.NewGuid(), UserId = Guid.NewGuid() };
			_postServiceMock.Setup(s => s.LikePost(likePostDto.PostId, likePostDto.UserId)).Returns<PostDetailsDto?>(null);

			var result = _controller.LikePost(likePostDto) as BadRequestObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(400));
				Assert.That(result.Value, Is.EqualTo("Error processing like"));
			});
		}

		[Test]
		public void UnlikePost_ErrorProcessingUnlike_ReturnsBadRequest()
		{
			var likePostDto = new LikePostDto { PostId = Guid.NewGuid(), UserId = Guid.NewGuid() };
			_postServiceMock.Setup(s => s.UnlikePost(likePostDto.PostId, likePostDto.UserId)).Returns<PostDetailsDto?>(null);

			var result = _controller.UnlikePost(likePostDto) as BadRequestObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(400));
				Assert.That(result.Value, Is.EqualTo("Error processing unlike"));
			});
		}
	}
}
