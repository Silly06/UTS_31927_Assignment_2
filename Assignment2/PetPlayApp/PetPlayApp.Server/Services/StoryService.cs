using PetPlayApp.Server.Db;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services;

public class StoryService(IRepositoryProviderService repositoryProvider) : IStoryService
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

    public void CreateStory(Story story)
    {
        _storyRepository.Add(story);
    }
}