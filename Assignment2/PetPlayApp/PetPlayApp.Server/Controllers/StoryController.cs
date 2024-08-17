using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Controllers;

[Route("stories")]
public class StoryController(IStoryService storyService) : Controller
{
    [HttpPost("CreateStory")]
    public IActionResult CreateStory([FromBody] StoryDetailsDto storyDetails)
    {
        if (storyDetails.StoryCreatorId == null)
        {
            return BadRequest("StoryCreatorId is required");
        }
        
        try
        {
            var story = new Story
            {
                Id = Guid.NewGuid(),
                DateTimePosted = DateTime.UtcNow,
                StoryCreatorId = storyDetails.StoryCreatorId,
                ImageData = storyDetails.ImageData
            };

            storyService.CreateStory(story);

            return Ok("Story created");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the story.");
        }
    }

    [HttpGet("GetAllStories")]
    public IActionResult GetAllStories()
    {
        var allStories = storyService.GetAllStoriesDetails();
        return Ok(allStories);
    }
}