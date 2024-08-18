using Moq;
using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services;
using PetPlayApp.Server.Services.Abstractions;
using Match = PetPlayApp.Server.Models.Match;

#nullable enable

namespace PetPlayApp.Test.Services
{
    [TestFixture]
    public class MatchServiceTests
    {
        private Mock<IRepositoryProviderService> _repositoryProviderMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IRepository<Match>> _matchRepositoryMock;
        private Mock<IRepository<Post>> _postRepositoryMock;
        private Mock<IRepository<User>> _userRepositoryMock;
        private MatchService _matchService;

        [SetUp]
        public void Setup()
        {
            _repositoryProviderMock = new Mock<IRepositoryProviderService>();
            _userServiceMock = new Mock<IUserService>();
            _matchRepositoryMock = new Mock<IRepository<Match>>();
            _postRepositoryMock = new Mock<IRepository<Post>>();
            _userRepositoryMock = new Mock<IRepository<User>>();

            _repositoryProviderMock.Setup(r => r.GetRepository<Match>()).Returns(_matchRepositoryMock.Object);
            _repositoryProviderMock.Setup(r => r.GetRepository<Post>()).Returns(_postRepositoryMock.Object);
            _repositoryProviderMock.Setup(r => r.GetRepository<User>()).Returns(_userRepositoryMock.Object);

            _matchService = new MatchService(_userServiceMock.Object, _repositoryProviderMock.Object);
        }

        [Test]
        public void UpdateMatchStatus_AwaitingResponse_UpdatesStatus()
        {
            var match = new Match
			{
                User1Response = UserResponse.Pending,
                User2Response = UserResponse.Accepted
            };

            _matchService.UpdateMatchStatus(match);

            Assert.That(match.OverallStatus, Is.EqualTo(MatchStatus.AwaitingResponse));
            _matchRepositoryMock.Verify(r => r.Update(match), Times.Once);
        }

        [Test]
        public void UpdateMatchStatus_MatchFailure_UpdatesStatus()
        {
            var match = new Match
            {
                User1Response = UserResponse.Rejected,
                User2Response = UserResponse.Accepted
            };

            _matchService.UpdateMatchStatus(match);

            Assert.That(match.OverallStatus, Is.EqualTo(MatchStatus.Failure));
            _matchRepositoryMock.Verify(r => r.Update(match), Times.Once);
        }

        [Test]
        public void GetAllMatches_ReturnsAllMatches()
        {
            var matches = new List<Match> { new Match(), new Match() };
            _matchRepositoryMock.Setup(r => r.GetAll()).Returns(matches.AsQueryable());

            var result = _matchService.GetAllMatches();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddMatch_ValidData_AddsMatch()
        {
            var user1 = new User { Id = Guid.NewGuid() };
            var user2 = new User { Id = Guid.NewGuid() };

            _matchService.AddMatch(user1, user2);

            _matchRepositoryMock.Verify(r => r.Add(It.IsAny<Match>()), Times.Once);
        }

        [Test]
        public void GetMatchesForUser_ValidUserId_ReturnsMatches()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            var matches = new List<Match>
            {
                new Match { User1 = user },
                new Match { User2 = user }
            };

            _userServiceMock.Setup(s => s.GetUser(userId)).Returns(user);
            _matchRepositoryMock.Setup(r => r.GetAll()).Returns(matches.AsQueryable());

            var result = _matchService.GetMatchesForUser(userId);

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CheckForMatch_ValidData_NoMatchFound()
        {
            var postId = Guid.NewGuid();
            var currentUserId = Guid.NewGuid();
            var post = new Post { Id = postId, PostCreator = new User { Id = Guid.NewGuid() } };
            var currentUser = new User { Id = currentUserId, CreatedPosts = new List<Post> { new Post() } };

            _postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
            _userRepositoryMock.Setup(r => r.GetById(currentUserId)).Returns(currentUser);
            _matchService.CheckForMatch(postId, currentUserId);

            _matchRepositoryMock.Verify(r => r.Add(It.IsAny<Match>()), Times.Never);
        }
    }
}
