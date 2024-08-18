using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions;

public interface IStoryService
{
    IEnumerable<StoryDetailsDto> GetAllStoriesDetails();

    StoryDetailsDto GetStoryDetails(Guid storyId);
    
    void CreateStory(Story story);

    void RemoveExpiredStories();
}