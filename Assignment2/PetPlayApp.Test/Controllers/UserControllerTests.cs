using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PetPlayApp.Server.Controllers;
using PetPlayApp.Server.Services.Abstractions;
using PetPlayApp.Server.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PetPlayApp.Test
{
	[TestFixture]
	public class UserControllerTests
	{
		private Mock<IUserService> _userServiceMock;
		private UserController _controller;

		[SetUp]
		public void Setup()
		{
			_userServiceMock = new Mock<IUserService>();
			_controller = new UserController(_userServiceMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_controller.Dispose();
		}

		[Test]
		public void Login_ValidCredentials_ReturnsOk()
		{
			var loginDto = new LoginDto { Username = "testuser", Password = "password" };
			var userId = Guid.NewGuid();
			var userPicture = new byte[] { 1, 2, 3 };

			_userServiceMock.Setup(s => s.TryValidateUser(loginDto.Username, loginDto.Password, out userId)).Returns(true);
			_userServiceMock.Setup(s => s.GetUserPicture(userId)).Returns(userPicture);

			var result = _controller.Login(loginDto) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo(new { UserId = userId, UserPfp = userPicture }));
			});
		}

		[Test]
		public void Login_InvalidCredentials_ReturnsUnauthorized()
		{
			var loginDto = new LoginDto { Username = "testuser", Password = "wrongpassword" };
			var userId = Guid.NewGuid();

			_userServiceMock.Setup(s => s.TryValidateUser(loginDto.Username, loginDto.Password, out userId)).Returns(false);

			var result = _controller.Login(loginDto) as UnauthorizedResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo(401));
		}

		[Test]
		public void GetUserDetails_ValidUserId_ReturnsOk()
		{
			var userId = Guid.NewGuid();
			var userDetails = new UserDetailsDto { UserId = userId, Username = "testuser" };

			_userServiceMock.Setup(s => s.GetUserDetails(userId)).Returns(userDetails);

			var result = _controller.GetUserDetails(userId) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo(userDetails));
			});
		}

		[Test]
		public async Task UpdateUserDetailsAsync_ValidData_ReturnsOk()
		{
			var userDetails = new UserDetailsDto { UserId = Guid.NewGuid(), Username = "testuser" };
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

			var result = await _controller.UpdateUserDetailsAsync(userDetails, fileMock.Object) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo("User details updated successfully."));
			});
		}

		[Test]
		public void CreateUser_ValidData_ReturnsOk()
		{
			var createUserDto = new CreateUserDto { Username = "testuser", Password = "password", Email = "test@example.com" };
			var userId = Guid.NewGuid();

			_userServiceMock.Setup(s => s.CreateUser(createUserDto.Username, createUserDto.Password, createUserDto.Email, createUserDto.Age, createUserDto.Bio, createUserDto.ProfilePicture));
			_userServiceMock.Setup(s => s.TryValidateUser(createUserDto.Username, createUserDto.Password, out userId)).Returns(true);

			var result = _controller.CreateUser(createUserDto) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo(new { UserId = userId }));
			});
		}

		[Test]
		public void GetUserPicture_ValidUserId_ReturnsOk()
		{
			var userId = Guid.NewGuid();
			var userPicture = new byte[] { 1, 2, 3 };

			_userServiceMock.Setup(s => s.GetUserPicture(userId)).Returns(userPicture);

			var result = _controller.GetUserPicture(userId) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo(userPicture));
			});
		}

		[Test]
		public void ResetPassword_ValidData_ReturnsOk()
		{
			var resetPasswordDto = new ResetPasswordDto { Email = "test@example.com", OldPassword = "oldpassword", NewPassword = "newpassword" };

			var result = _controller.ResetPassword(resetPasswordDto) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo("Password reset successfully."));
			});
		}
	}
}
