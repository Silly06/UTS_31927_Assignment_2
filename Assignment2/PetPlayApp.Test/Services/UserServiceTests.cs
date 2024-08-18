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
    public class UserServiceTests
    {
        private Mock<IRepositoryProviderService> _repositoryProviderMock;
        private Mock<IRepository<User>> _userRepositoryMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _repositoryProviderMock = new Mock<IRepositoryProviderService>();
            _userRepositoryMock = new Mock<IRepository<User>>();

            _repositoryProviderMock.Setup(r => r.GetRepository<User>()).Returns(_userRepositoryMock.Object);

            _userService = new UserService(_repositoryProviderMock.Object);
        }

        [Test]
        public void GetAllUsers_ReturnsAllUsers()
        {
            var users = new List<User> { new User(), new User() };
            _userRepositoryMock.Setup(r => r.GetAll()).Returns(users.AsQueryable());

            var result = _userService.GetAllUsers();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void TryValidateUser_ValidCredentials_ReturnsTrue()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, UserName = "test", Password = "password" };
            _userRepositoryMock.Setup(r => r.GetAll()).Returns(new List<User> { user }.AsQueryable());

            var result = _userService.TryValidateUser("test", "password", out var validatedUserId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(validatedUserId, Is.EqualTo(userId));
            });
        }

        [Test]
        public void TryValidateUser_InvalidCredentials_ReturnsFalse()
        {
            _userRepositoryMock.Setup(r => r.GetAll()).Returns(new List<User>().AsQueryable());

            var result = _userService.TryValidateUser("test", "password", out var validatedUserId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(validatedUserId, Is.EqualTo(Guid.Empty));
            });
        }

        [Test]
        public void RemoveUser_ValidId_RemovesUser()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

            _userService.RemoveUser(userId);

            _userRepositoryMock.Verify(r => r.Remove(user), Times.Once);
        }

        [Test]
        public void GetUser_ValidId_ReturnsUser()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

            var result = _userService.GetUser(userId);

            Assert.That(result, Is.EqualTo(user));
        }

        [Test]
        public void GetUser_InvalidId_ReturnsNull()
        {
#pragma warning disable CS8600 // Testing null for nullable type
            _userRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((User)null);
#pragma warning restore CS8600 // Testing null for nullable type

            var result = _userService.GetUser(Guid.NewGuid());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetUserDetails_ValidId_ReturnsUserDetailsDto()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                UserName = "test",
                Email = "test@example.com",
                Age = 30,
                Bio = "bio",
                ProfilePictureData = new byte[0],
                UserStatus = UserStatus.Unlisted,
                Interest = UserInterest.Birds
            };
            _userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

            var result = _userService.GetUserDetails(userId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Username, Is.EqualTo("test"));
                Assert.That(result.Email, Is.EqualTo("test@example.com"));
                Assert.That(result.Age, Is.EqualTo(30));
                Assert.That(result.Bio, Is.EqualTo("bio"));
                Assert.That(result.ProfilePicture, Is.EqualTo(new byte[0]));
                Assert.That(result.Status, Is.EqualTo(UserStatus.Unlisted));
                Assert.That(result.Interest, Is.EqualTo(UserInterest.Birds));
            });
        }

        [Test]
        public void UpdateUserDetails_InvalidId_ThrowsException()
        {
#pragma warning disable CS8600 // Testing null for nullable type
            _userRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((User)null);
#pragma warning restore CS8600 // Testing null for nullable type

            Assert.Throws<Exception>(() => _userService.UpdateUserDetails(Guid.NewGuid(), "newUsername", "newEmail", 25, "newBio", UserStatus.Matched, UserInterest.Amphibians, new byte[0]));
        }

        [Test]
        public void CreateUser_ExistingUser_ThrowsException()
        {
            var user = new User { UserName = "existingUser", Email = "existingEmail" };
            _userRepositoryMock.Setup(r => r.GetAll()).Returns(new List<User> { user }.AsQueryable());

            Assert.Throws<Exception>(() => _userService.CreateUser("existingUser", "password", "existingEmail", 25, "bio", new byte[0]));
        }

        [Test]
        public void GetUserPicture_ValidId_ReturnsPicture()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, ProfilePictureData = new byte[0] };
            _userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);

            var result = _userService.GetUserPicture(userId);

            Assert.That(result, Is.EqualTo(new byte[0]));
        }

        [Test]
        public void SearchUsers_ValidQuery_ReturnsUserSearchDtoList()
        {
            var currentUserId = Guid.NewGuid();
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Bio = "bio1", Interest = UserInterest.Amphibians },
                new User { Id = Guid.NewGuid(), Bio = "bio2", Interest = UserInterest.Birds }
            };
            _userRepositoryMock.Setup(r => r.GetAll()).Returns(users.AsQueryable());

            var result = _userService.SearchUsers(currentUserId, "bio");

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ResetPassword_ValidData_ResetsPassword()
        {
            var user = new User { Email = "email", Password = "oldPassword" };
            _userRepositoryMock.Setup(r => r.GetAll()).Returns(new List<User> { user }.AsQueryable());

            _userService.ResetPassword("email", "oldPassword", "newPassword");

            _userRepositoryMock.Verify(r => r.Update(It.Is<User>(u => u.Password == "newPassword")), Times.Once);
        }

        [Test]
        public void ResetPassword_InvalidData_ThrowsException()
        {
            _userRepositoryMock.Setup(r => r.GetAll()).Returns(new List<User>().AsQueryable());

            Assert.Throws<Exception>(() => _userService.ResetPassword("email", "oldPassword", "newPassword"));
        }
    }
}

