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

#nullable enable

namespace PetPlayApp.Test.Services
{
    [TestFixture]
    public class StoryServiceTests
    {
        private Mock<IRepositoryProviderService> _repositoryProviderMock;
        private Mock<IRepository<Story>> _storyRepositoryMock;
        private Mock<IUserService> _userServiceMock;
        private StoryService _storyService;

        [SetUp]
        public void Setup()
        {
            _repositoryProviderMock = new Mock<IRepositoryProviderService>();
            _storyRepositoryMock = new Mock<IRepository<Story>>();
            _userServiceMock = new Mock<IUserService>();

            _repositoryProviderMock.Setup(r => r.GetRepository<Story>()).Returns(_storyRepositoryMock.Object);

            _storyService = new StoryService(_repositoryProviderMock.Object, _userServiceMock.Object);
        }

        [Test]
        public void GetStoryDetails_InvalidStoryId_ReturnsNull()
        {
            var storyId = Guid.NewGuid();

#pragma warning disable CS8600 // Testing null for nullable type
            _storyRepositoryMock.Setup(r => r.GetById(storyId)).Returns((Story)null);
#pragma warning restore CS8600 // Testing null for nullable type

            var result = _storyService.GetStoryDetails(storyId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RemoveExpiredStories_RemovesExpiredStoriesFromRepository()
        {
            var expiredStory = new Story { Id = Guid.NewGuid(), DateTimePosted = DateTime.UtcNow.AddHours(-25) };
            var validStory = new Story { Id = Guid.NewGuid(), DateTimePosted = DateTime.UtcNow.AddHours(-23) };
            var stories = new List<Story> { expiredStory, validStory };

            _storyRepositoryMock.Setup(r => r.GetAll()).Returns(stories.AsQueryable());

            _storyService.RemoveExpiredStories();

            _storyRepositoryMock.Verify(r => r.Remove(expiredStory), Times.Once);
            _storyRepositoryMock.Verify(r => r.Remove(validStory), Times.Never);
        }
    }
}
