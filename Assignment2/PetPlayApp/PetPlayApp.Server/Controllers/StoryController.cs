using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Controllers;

[Route("stories")]
public class StoryController(IStoryService storyService) : Controller
{
    [HttpPost("CreateStory")]
    public async Task<IActionResult> CreateStory([FromForm] Guid? storyCreatorId, [FromForm] IFormFile? file)
    {
        if (storyCreatorId == null)
        {
            return BadRequest("StoryCreatorId is required");
        }
        
        try
        {
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await file!.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }
            
            var story = new Story
            {
                Id = Guid.NewGuid(),
                DateTimePosted = DateTime.UtcNow,
                StoryCreatorId = storyCreatorId,
                ImageData = imageData
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

    [HttpGet("GetStoryDetails")]
    public IActionResult GetStoryDetails([FromQuery] Guid storyId)
    {
        var storyDetails = storyService.GetStoryDetails(storyId);
        return Ok(storyDetails);
    }
}