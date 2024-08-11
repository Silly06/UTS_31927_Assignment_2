using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;
using System.Text.Json;
using System.Net;

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
        public HttpResponseMessage GetRecentPosts()
        {
            var posts = _postRepository.GetAll().OrderBy(x => x.DateTimePosted);
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(posts)),
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }

		[HttpGet("GetUserPosts")]
		public HttpResponseMessage GetUserPosts([FromBody] Guid userid)
		{
			var posts = _postRepository.GetAll().Where(x => x.PostCreatorId == userid).OrderBy(x => x.DateTimePosted);
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