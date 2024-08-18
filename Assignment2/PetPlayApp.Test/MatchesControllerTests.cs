using Moq;
using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Services.Abstractions;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Db;

namespace PetPlayApp.Test
{
	[TestFixture]
	public class MatchesControllerTests
	{
		private Mock<IMatchService> _matchServiceMock;
		private Mock<IRepository<User>> _userRepositoryMock;
		private MatchesController _controller;

		[SetUp]
		public void Setup()
		{
			_matchServiceMock = new Mock<IMatchService>();
			_userRepositoryMock = new Mock<IRepository<User>>();
			var repositoryProviderServiceMock = new Mock<IRepositoryProviderService>();
			repositoryProviderServiceMock.Setup(r => r.GetRepository<User>()).Returns(_userRepositoryMock.Object);
			_controller = new MatchesController(_matchServiceMock.Object, repositoryProviderServiceMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_controller.Dispose();
		}

		[Test]
		public void GetMatchesForUser_ValidUserId_ReturnsOk()
		{
			var userId = Guid.NewGuid();
			var matches = new List<Server.Models.Match>
			{
				new Server.Models.Match { User1Id = Guid.NewGuid(), User2Id = Guid.NewGuid(), OverallStatus = MatchStatus.Success, User1Response = UserResponse.Accepted, User2Response = UserResponse.Accepted }
			};
			_matchServiceMock.Setup(s => s.GetMatchesForUser(userId)).Returns(matches);
			_userRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns(new User { UserName = "TestUser" });

			var result = _controller.GetMatchesForUser(userId) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo(200));
			var matchDetails = result.Value as List<MatchDetailsDto>;
			Assert.That(matchDetails, Is.Not.Null);
			Assert.That(matchDetails.Count, Is.EqualTo(1));
		}

		[Test]
		public void GetMatchesForUser_NoMatches_ReturnsNotFound()
		{
			var userId = Guid.NewGuid();
			_matchServiceMock.Setup(s => s.GetMatchesForUser(userId)).Returns(new List<Server.Models.Match>());

			var result = _controller.GetMatchesForUser(userId) as NotFoundResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo(404));
		}

		[Test]
		public void CheckForMatch_ValidData_CallsService()
		{
			var likePostDto = new LikePostDto { PostId = Guid.NewGuid(), UserId = Guid.NewGuid() };

			_controller.CheckForMatch(likePostDto);

			_matchServiceMock.Verify(s => s.CheckForMatch(likePostDto.PostId, likePostDto.UserId), Times.Once);
		}
	}
}
