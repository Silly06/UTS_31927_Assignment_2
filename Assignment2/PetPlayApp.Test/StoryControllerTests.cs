using Moq;
using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Controllers;
using PetPlayApp.Server.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using PetPlayApp.Server.Dto;

namespace PetPlayApp.Test
{
	[TestFixture]
	public class StoryControllerTests
	{
		private Mock<IStoryService> _storyServiceMock;
		private StoryController _controller;

		[SetUp]
		public void Setup()
		{
			_storyServiceMock = new Mock<IStoryService>();
			_controller = new StoryController(_storyServiceMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_controller.Dispose();
		}

		[Test]
		public async Task CreateStory_ValidData_ReturnsOk()
		{
			var storyCreatorId = Guid.NewGuid();
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

			var result = await _controller.CreateStory(storyCreatorId, fileMock.Object) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(200));
				Assert.That(result.Value, Is.EqualTo("Story created"));
			});
		}

		[Test]
		public async Task CreateStory_MissingStoryCreatorId_ReturnsBadRequest()
		{
			var fileMock = new Mock<IFormFile>();

			var result = await _controller.CreateStory(null, fileMock.Object) as BadRequestObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(400));
				Assert.That(result.Value, Is.EqualTo("StoryCreatorId is required"));
			});
		}

		[Test]
		public async Task CreateStory_ExceptionThrown_ReturnsServerError()
		{
			var storyCreatorId = Guid.NewGuid();
			var fileMock = new Mock<IFormFile>();
			fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>())).Throws(new Exception());

			var result = await _controller.CreateStory(storyCreatorId, fileMock.Object) as ObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(result.StatusCode, Is.EqualTo(500));
				Assert.That(result.Value, Is.EqualTo("An error occurred while creating the story."));
			});
		}

		[Test]
		public void GetAllStories_ReturnsOk()
		{
			_storyServiceMock.Setup(s => s.GetAllStoriesDetails()).Returns(new List<StoryDetailsDto>());

			var result = _controller.GetAllStories() as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo(200));
		}

		[Test]
		public void GetStoryDetails_ValidStoryId_ReturnsOk()
		{
			var storyId = Guid.NewGuid();
			_storyServiceMock.Setup(s => s.GetStoryDetails(storyId)).Returns(new StoryDetailsDto());

			var result = _controller.GetStoryDetails(storyId) as OkObjectResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo(200));
		}
	}
}
