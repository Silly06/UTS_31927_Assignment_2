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
    public class CommentServiceTests
    {
        private Mock<IRepositoryProviderService> _repositoryProviderMock;
        private Mock<INotificationService> _notificationServiceMock;
        private Mock<IRepository<Comment>> _commentRepositoryMock;
        private Mock<IRepository<Post>> _postRepositoryMock;
        private Mock<IRepository<User>> _userRepositoryMock;
        private CommentService _commentService;

        [SetUp]
        public void Setup()
        {
            _repositoryProviderMock = new Mock<IRepositoryProviderService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _commentRepositoryMock = new Mock<IRepository<Comment>>();
            _postRepositoryMock = new Mock<IRepository<Post>>();
            _userRepositoryMock = new Mock<IRepository<User>>();

            _repositoryProviderMock.Setup(r => r.GetRepository<Comment>()).Returns(_commentRepositoryMock.Object);
            _repositoryProviderMock.Setup(r => r.GetRepository<Post>()).Returns(_postRepositoryMock.Object);
            _repositoryProviderMock.Setup(r => r.GetRepository<User>()).Returns(_userRepositoryMock.Object);

            _commentService = new CommentService(_repositoryProviderMock.Object, _notificationServiceMock.Object);
        }

        [Test]
        public void AddComment_ValidData_AddsComment()
        {
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var content = "Test comment";
			var postCreatorId = Guid.NewGuid();
			var postCreator = new User { Id = postCreatorId };
			var post = new Post { Id = postId, PostCreatorId = postCreatorId, PostCreator = postCreator };
            var user = new User { Id = userId };

            _postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
			_userRepositoryMock.Setup(r => r.GetById(postCreatorId)).Returns(postCreator);
			_userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

            _commentService.AddComment(postId, userId, content);

            _commentRepositoryMock.Verify(r => r.Add(It.IsAny<Comment>()), Times.Once);
            _notificationServiceMock.Verify(n => n.NotifyCommentCreated(postId, userId, postCreatorId), Times.Once);
        }

        [Test]
        public void AddComment_InvalidPostId_ThrowsArgumentException()
        {
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var content = "Test comment";

#pragma warning disable CS8600 // Testing null for nullable type
            _postRepositoryMock.Setup(r => r.GetById(postId)).Returns((Post)null);
#pragma warning restore CS8600 // Testing null for nullable type

            Assert.Throws<ArgumentException>(() => _commentService.AddComment(postId, userId, content));
        }

        [Test]
        public void AddComment_InvalidUserId_ThrowsArgumentException()
        {
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var content = "Test comment";
            var post = new Post { Id = postId };

            _postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
#pragma warning disable CS8600 // Testing null for nullable type
            _userRepositoryMock.Setup(r => r.GetById(userId)).Returns((User)null);
#pragma warning restore CS8600 // Testing null for nullable type

            Assert.Throws<ArgumentException>(() => _commentService.AddComment(postId, userId, content));
        }

        [Test]
        public void AddComment_EmptyContent_ThrowsArgumentException()
        {
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var content = "";

            Assert.Throws<ArgumentException>(() => _commentService.AddComment(postId, userId, content));
        }

        [Test]
        public void GetCommentsForPost_ValidPostId_ReturnsComments()
        {
            var postId = Guid.NewGuid();
            var comments = new List<Comment>
            {
                new Comment { PostId = postId, CreatedAt = DateTime.UtcNow },
                new Comment { PostId = postId, CreatedAt = DateTime.UtcNow.AddMinutes(-1) }
            };

            _commentRepositoryMock.Setup(r => r.GetAll()).Returns(comments.AsQueryable());

            var result = _commentService.GetCommentsForPost(postId);

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void LikeComment_ValidData_AddsLike()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var comment = new Comment { Id = commentId, Likes = new List<User>() };
            var user = new User { Id = userId };

            _commentRepositoryMock.Setup(r => r.GetById(commentId)).Returns(comment);
            _userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

            _commentService.LikeComment(commentId, userId);

            Assert.That(comment.Likes, Contains.Item(user));
            _commentRepositoryMock.Verify(r => r.Update(comment), Times.Once);
        }

        [Test]
        public void LikeComment_InvalidCommentId_ThrowsArgumentException()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();

#pragma warning disable CS8600 // Testing null for nullable type
            _commentRepositoryMock.Setup(r => r.GetById(commentId)).Returns((Comment)null);
#pragma warning restore CS8600 // Testing null for nullable type

            Assert.Throws<ArgumentException>(() => _commentService.LikeComment(commentId, userId));
        }

        [Test]
        public void LikeComment_InvalidUserId_ThrowsArgumentException()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var comment = new Comment { Id = commentId };

            _commentRepositoryMock.Setup(r => r.GetById(commentId)).Returns(comment);
#pragma warning disable CS8600 // Testing null for nullable type
            _userRepositoryMock.Setup(r => r.GetById(userId)).Returns((User)null);
#pragma warning restore CS8600 // Testing null for nullable type

            Assert.Throws<ArgumentException>(() => _commentService.LikeComment(commentId, userId));
        }

        [Test]
        public void UnlikeComment_ValidData_RemovesLike()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            var comment = new Comment { Id = commentId, Likes = new List<User> { user } };

            _commentRepositoryMock.Setup(r => r.GetById(commentId)).Returns(comment);
            _userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

            _commentService.UnlikeComment(commentId, userId);

            Assert.That(comment.Likes, Does.Not.Contain(user));
            _commentRepositoryMock.Verify(r => r.Update(comment), Times.Once);
        }

        [Test]
        public void UnlikeComment_InvalidCommentId_ThrowsArgumentException()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();

#pragma warning disable CS8600 // Testing null for nullable type
            _commentRepositoryMock.Setup(r => r.GetById(commentId)).Returns((Comment)null);
#pragma warning restore CS8600 // Testing null for nullable type

            Assert.Throws<ArgumentException>(() => _commentService.UnlikeComment(commentId, userId));
        }

        [Test]
        public void UnlikeComment_InvalidUserId_ThrowsArgumentException()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var comment = new Comment { Id = commentId };

            _commentRepositoryMock.Setup(r => r.GetById(commentId)).Returns(comment);
#pragma warning disable CS8600 // Testing null for nullable type
            _userRepositoryMock.Setup(r => r.GetById(userId)).Returns((User)null);
#pragma warning restore CS8600 // Testing null for nullable type

            Assert.Throws<ArgumentException>(() => _commentService.UnlikeComment(commentId, userId));
        }
    }
}
