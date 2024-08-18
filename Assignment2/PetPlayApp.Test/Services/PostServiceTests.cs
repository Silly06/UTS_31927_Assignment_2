using Moq;
using NUnit.Framework;
using PetPlayApp.Server.Db;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services;
using PetPlayApp.Server.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetPlayApp.Test.Services
{
	[TestFixture]
	public class PostServiceTests
	{
		private Mock<IRepositoryProviderService> _repositoryProviderMock;
		private Mock<IRepository<Post>> _postRepositoryMock;
		private Mock<IRepository<User>> _userRepositoryMock;
		private Mock<INotificationService> _notificationServiceMock;
		private PostService _postService;

		[SetUp]
		public void Setup()
		{
			_repositoryProviderMock = new Mock<IRepositoryProviderService>();
			_postRepositoryMock = new Mock<IRepository<Post>>();
			_userRepositoryMock = new Mock<IRepository<User>>();
			_notificationServiceMock = new Mock<INotificationService>();

			_repositoryProviderMock.Setup(r => r.GetRepository<Post>()).Returns(_postRepositoryMock.Object);
			_repositoryProviderMock.Setup(r => r.GetRepository<User>()).Returns(_userRepositoryMock.Object);

			_postService = new PostService(_repositoryProviderMock.Object, _notificationServiceMock.Object);
		}

		[Test]
		public void LikePost_InvalidPostId_ReturnsNull()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();

#pragma warning disable CS8600 // Testing null for nullable type
			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns((Post)null);
#pragma warning restore CS8600 // Testing null for nullable type

			var result = _postService.LikePost(postId, userId);

			Assert.That(result, Is.Null);
		}

		[Test]
		public void LikePost_InvalidUserId_ReturnsNull()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var post = new Post { Id = postId };

			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
#pragma warning disable CS8600 // Testing null for nullable type
			_userRepositoryMock.Setup(r => r.GetById(userId)).Returns((User)null);
#pragma warning restore CS8600 // Testing null for nullable type

			var result = _postService.LikePost(postId, userId);

			Assert.That(result, Is.Null);
		}

		[Test]
		public void LikePost_AlreadyLiked_ReturnsPostDetailsDto()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var user = new User { Id = userId };
			var post = new Post { Id = postId, Likes = new List<User> { user } };

			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
			_userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

			var result = _postService.LikePost(postId, userId);

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.PostId, Is.EqualTo(postId));
				Assert.That(result.LikesCount, Is.EqualTo(1));
				Assert.That(result.LikedByUser, Is.True);
			});
		}

		[Test]
		public void UnlikePost_InvalidPostId_ReturnsNull()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();

#pragma warning disable CS8600 // Testing null for nullable type
			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns((Post)null);
#pragma warning restore CS8600 // Testing null for nullable type

			var result = _postService.UnlikePost(postId, userId);

			Assert.That(result, Is.Null);
		}

		[Test]
		public void UnlikePost_InvalidUserId_ReturnsNull()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var post = new Post { Id = postId };

			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
#pragma warning disable CS8600 // Testing null for nullable type
			_userRepositoryMock.Setup(r => r.GetById(userId)).Returns((User)null);
#pragma warning restore CS8600 // Testing null for nullable type

			var result = _postService.UnlikePost(postId, userId);

			Assert.That(result, Is.Null);
		}

		[Test]
		public void UnlikePost_NotLiked_ReturnsPostDetailsDto()
		{
			var postId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var user = new User { Id = userId };
			var post = new Post { Id = postId, Likes = new List<User>() };

			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
			_userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

			var result = _postService.UnlikePost(postId, userId);

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.PostId, Is.EqualTo(postId));
				Assert.That(result.LikesCount, Is.EqualTo(0));
				Assert.That(result.LikedByUser, Is.False);
			});
		}

		[Test]
		public void GetRecentPosts_ValidPage_ReturnsPostIds()
		{
			var posts = new List<Post>
			{
				new Post { Id = Guid.NewGuid(), DateTimePosted = DateTime.UtcNow.AddMinutes(-1) },
				new Post { Id = Guid.NewGuid(), DateTimePosted = DateTime.UtcNow.AddMinutes(-2) }
			};

			_postRepositoryMock.Setup(r => r.GetAll()).Returns(posts.AsQueryable());

			var result = _postService.GetRecentPosts(1);

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public void GetUserPosts_ValidUserIdAndPage_ReturnsPostIds()
		{
			var userId = Guid.NewGuid();
			var posts = new List<Post>
			{
				new Post { Id = Guid.NewGuid(), PostCreatorId = userId, DateTimePosted = DateTime.UtcNow.AddMinutes(-1) },
				new Post { Id = Guid.NewGuid(), PostCreatorId = userId, DateTimePosted = DateTime.UtcNow.AddMinutes(-2) }
			};

			_postRepositoryMock.Setup(r => r.GetAll()).Returns(posts.AsQueryable());

			var result = _postService.GetUserPosts(1, userId);

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public void GetPost_ValidPostId_ReturnsPost()
		{
			var postId = Guid.NewGuid();
			var post = new Post { Id = postId };

			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);

			var result = _postService.GetPost(postId);

			Assert.That(result, Is.EqualTo(post));
		}

		[Test]
		public void GetPost_InvalidPostId_ReturnsNull()
		{
			var postId = Guid.NewGuid();

#pragma warning disable CS8600 // Testing null for nullable type
			_postRepositoryMock.Setup(r => r.GetById(postId)).Returns((Post)null);
#pragma warning restore CS8600 // Testing null for nullable type

			var result = _postService.GetPost(postId);

			Assert.That(result, Is.Null);
		}

		[Test]
		public void AddPost_ValidPost_AddsPost()
		{
			var post = new Post { Id = Guid.NewGuid() };

			_postService.AddPost(post);

			_postRepositoryMock.Verify(r => r.Add(post), Times.Once);
		}
	}
}
