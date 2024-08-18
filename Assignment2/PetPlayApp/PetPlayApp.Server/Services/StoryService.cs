using PetPlayApp.Server.Db;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services;

public class StoryService(IRepositoryProviderService repositoryProvider, IUserService userService) : IStoryService
{
    private readonly IRepository<Story> _storyRepository = repositoryProvider.GetRepository<Story>();

    public IEnumerable<StoryDetailsDto> GetAllStoriesDetails()
    {
        return _storyRepository.GetAll().Select(x => new StoryDetailsDto
        {
            StoryId = x.Id,
            StoryCreatorId = x.StoryCreatorId,
            ImageData = x.ImageData
        });
    }

    public StoryDetailsDto GetStoryDetails(Guid storyId)
    {
        var story = _storyRepository.GetById(storyId);
        var username = userService.GetUserDetails(story?.StoryCreatorId ?? Guid.Empty).Username;

        return new StoryDetailsDto
        {
            StoryId = story?.Id,
            StoryCreatorId = story?.StoryCreatorId,
            StoryCreatorName = username,
            ImageData = story?.ImageData,
            DateTimePosted = story?.DateTimePosted
        };
    }

    public void CreateStory(Story story)
    {
        _storyRepository.Add(story);
    }

    public void RemoveExpiredStories()
    {
        var expirationTime = DateTime.UtcNow.AddHours(-24);
        var expiredStories = _storyRepository
            .GetAll()
            .Where(x => x.DateTimePosted < expirationTime)
            .ToList();
        if (expiredStories.Count == 0) return;
        
        foreach (var story in expiredStories)
        {
            _storyRepository.Remove(story);
        }
    }
}