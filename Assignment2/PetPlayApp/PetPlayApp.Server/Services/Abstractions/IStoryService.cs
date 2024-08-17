using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions;

public interface IStoryService
{
    IEnumerable<StoryDetailsDto> GetAllStoriesDetails();
    
    void CreateStory(Story story);
}