using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;
using System.Text.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
		public HttpResponseMessage GetRecentPosts([FromBody] int page)
		{
			var postsIds = _postRepository.GetAll()
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
		public HttpResponseMessage GetPostDetails([FromBody] Guid postid)
		{
			var posts = _postRepository.GetById(postid);
			var response = new HttpResponseMessage
			{
				Content = new StringContent(JsonSerializer.Serialize(posts)),
				StatusCode = HttpStatusCode.OK
			};
			return response;
		}

		[HttpPost("NewPost")]
        public HttpResponseMessage CreatePost([FromBody] PostRequestModel postRequest)
		{
            if (postRequest.PostCreatorId == null)
            {
				return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "PostCreatorId is missing" };
			}
			if (postRequest.Description == null)
			{
				return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "Description is missing" };
			}
			var post = new Post
			{
				DateTimePosted = DateTime.Now,
				PostCreatorId = postRequest.PostCreatorId,
				Description = postRequest.Description
			};
			_postRepository.Add(post);
			var response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK
			};
			return response;
		}
	}
}