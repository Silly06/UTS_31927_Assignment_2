using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Db.Services;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Controllers
{
    [Route("posts")]
    public class PostsController : Controller
    {
        private readonly PostService postService;

        public PostsController(PostService postService)
        {
			this.postService = postService;
		}

		[HttpGet("GetRecentPosts")]
		public IActionResult GetRecentPosts([FromQuery] int page)
		{
			var postsIds = postService.GetRecentPosts(page);
			return Ok(postsIds);
		}

		[HttpGet("GetUserPosts")]
		public IActionResult GetUserPosts([FromBody] int page, [FromBody] Guid userid)
		{
			var postsIds = postService.GetUserPosts(page, userid);
			return Ok(postsIds);
		}

		[HttpGet("GetPostDetails")]
		public IActionResult GetPostDetails([FromQuery] Guid postid)
		{
			var post = postService.GetPost(postid);
			return Ok(post);
		}

		[HttpPost("NewPost")]
		public async Task<IActionResult> NewPost([FromForm] IFormFile? image, [FromForm] string? description, [FromForm] Guid? postCreatorId)
		{
			if (image == null || image.Length == 0)
				return BadRequest("Image is required");
			if (description == null)
				return BadRequest("Description is required");
			if (postCreatorId == null)
				return BadRequest("Post creator id is required");

			byte[] imageData;
			using (var memoryStream = new MemoryStream())
			{
				await image.CopyToAsync(memoryStream);
				imageData = memoryStream.ToArray();
			}

			var post = new Post
			{
				Id = Guid.NewGuid(),
				DateTimePosted = DateTime.Now,
				PostCreatorId = postCreatorId,
				Description = description,
				ImageData = imageData
			};

			postService.AddPost(post);

			return Ok(post);
		}
	}
}