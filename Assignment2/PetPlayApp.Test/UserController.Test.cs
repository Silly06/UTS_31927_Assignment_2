using Moq;
using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Controllers;
using PetPlayApp.Server.Services.Abstractions;
using PetPlayApp.Server.Dto;
using Microsoft.AspNetCore.Http;

namespace PetPlayApp.Test;

[TestFixture]
public class UserControllerTests
{
    private Mock<IUserService> _userServiceMock;
    private UserController _controller;

    [SetUp]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UserController(_userServiceMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
        _controller = null;
        _userServiceMock = null;
    }

    [Test]
    public void Login_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        var userId = Guid.NewGuid();
        var userProfilePicture = new byte[] { 1, 2, 3 };

        _userServiceMock.Setup(service => service.TryValidateUser(loginDto.Username, loginDto.Password, out userId))
            .Returns(true);

        _userServiceMock.Setup(service => service.GetUserPicture(userId)).Returns(userProfilePicture);

        // Act
        var result = _controller.Login(loginDto) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public void Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        Guid outUserId;
        _userServiceMock.Setup(service => service.TryValidateUser(loginDto.Username, loginDto.Password, out outUserId)).Returns(false);

        // Act
        var result = _controller.Login(loginDto) as UnauthorizedResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
    }

    [Test]
    public void GetUserDetails_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userDetails = new UserDetailsDto { Username = "testuser", Email = "test@example.com" };
        _userServiceMock.Setup(service => service.GetUserDetails(userId)).Returns(userDetails);

        // Act
        var result = _controller.GetUserDetails(userId) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual(userDetails, result.Value);
    }

    [Test]
    public async Task UpdateUserDetailsAsync_WithValidDetails_ReturnsOkResult()
    {
        // Arrange
        var userDetails = new UserDetailsDto 
        { 
            UserId = Guid.NewGuid(), 
            Username = "testuser", 
            Email = "test@example.com" 
        };
        var userImage = new byte[] { 1, 2, 3 };

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(userImage));
        fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
            .Callback<Stream, System.Threading.CancellationToken>((s, _) =>
            {
                using (var stream = fileMock.Object.OpenReadStream())
                {
                    stream.CopyTo(s);
                }
            });

        // Act
        var result = await _controller.UpdateUserDetailsAsync(userDetails, fileMock.Object) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);

        _userServiceMock.Verify(service => service.UpdateUserDetails(
            userDetails.UserId,
            userDetails.Username,
            userDetails.Email,
            userDetails.Age,
            userDetails.Bio,
            userDetails.Status,
            userDetails.Interest,
            It.Is<byte[]>(data => data.SequenceEqual(userImage))
        ), Times.Once);
    }

    [Test]
    public void CreateUser_WithValidDetails_ReturnsOkResult()
    {
        // Arrange
        var createUserDto = new CreateUserDto 
        { 
            Username = "newuser", 
            Password = "newpassword", 
            Email = "new@example.com" 
        };
        var userId = Guid.NewGuid();
        _userServiceMock.Setup(service => service.CreateUser(createUserDto.Username, createUserDto.Password, createUserDto.Email, createUserDto.Age, createUserDto.Bio, createUserDto.ProfilePicture));
        _userServiceMock.Setup(service => service.TryValidateUser(createUserDto.Username, createUserDto.Password, out userId)).Returns(true);

        // Act
        var result = _controller.CreateUser(createUserDto) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public void SearchUsers_WithValidQuery_ReturnsOkResult()
    {
        // Arrange
        var currentUserId = Guid.NewGuid();
        var query = "test";
        var searchResults = new List<UserSearchDto>
        {
            new UserSearchDto { UserId = Guid.NewGuid(), Username = "searchuser", ProfilePicture = new byte[] { 1, 2, 3 } }
        };
        _userServiceMock.Setup(service => service.SearchUsers(currentUserId, query)).Returns(searchResults);

        // Act
        var result = _controller.SearchUsers(currentUserId, query) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual(searchResults, result.Value);
    }

    [Test]
    public void GetUserPicture_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var picture = new byte[] { 1, 2, 3 };
        _userServiceMock.Setup(service => service.GetUserPicture(userId)).Returns(picture);

        // Act
        var result = _controller.GetUserPicture(userId) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual(picture, result.Value);
    }

    [Test]
    public void ResetPassword_WithValidDetails_ReturnsOkResult()
    {
        // Arrange
        var resetPasswordDto = new ResetPasswordDto
        {
            Email = "test@example.com",
            OldPassword = "oldpassword",
            NewPassword = "newpassword"
        };

        // Act
        var result = _controller.ResetPassword(resetPasswordDto) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public void Login_WithEmptyUsername_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "", Password = "password" };

        // Act
        var result = _controller.Login(loginDto) as BadRequestObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(400, result.StatusCode);
        Assert.AreEqual("Username or password was empty or missing", result.Value);
    }
    
    internal class CreateUserResult
    {
        public Guid UserId { get; set; }
    }
}
