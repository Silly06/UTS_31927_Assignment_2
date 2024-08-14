using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace PetPlayApp.Server.Controllers
{
    [Route("posts")]
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly PostRepository _postRepository;

        public PostsController(ILogger<PostsController> logger, PostRepository postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
        }

		[HttpGet("GetRecentPosts")]
		public IActionResult GetRecentPosts([FromQuery] int page)
		{
			var postsIds = _postRepository.GetAll()
				.OrderBy(x => x.DateTimePosted)
				.Take(page * 10)
				.TakeLast(10)
				.OrderBy(x => x.DateTimePosted)
				.Select(x => x.Id)
				.ToList();

			return Ok(postsIds);
		}

		[HttpGet("GetUserPosts")]
		public HttpResponseMessage GetUserPosts([FromBody] int page, [FromBody] Guid userid)
		{
			var postsIds = _postRepository.GetAll()
				.Where(x => x.PostCreatorId == userid)
				.OrderBy(x => x.DateTimePosted)
				.Take(page * 10)
				.TakeLast(10)
				.OrderBy(x => x.DateTimePosted)
				.Select(x => x.Id)
				.ToList();

			var response = new HttpResponseMessage
			{
				Content = new StringContent(JsonSerializer.Serialize(postsIds)),
				StatusCode = HttpStatusCode.OK
			};
			return response;
		}

		[HttpGet("GetPostDetails")]
		public IActionResult GetPostDetails([FromQuery] Guid postid)
		{
			var post = _postRepository.GetById(postid);
			return Ok(post);
		}

		[HttpPost("NewPost")]
		public async Task<IActionResult> NewPost([FromForm] IFormFile image, [FromForm] string description, [FromForm] Guid postCreatorId)
		{
			if (image == null || image.Length == 0)
				return BadRequest("Image is required");

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

			_postRepository.Add(post);

			return Ok(post);
		}
	}
}