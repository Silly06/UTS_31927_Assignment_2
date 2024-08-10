using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Db.Repos;
using System.Text.Json;
using System.Net;

namespace PetPlayApp.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostRepository _postRepository;

        public HomeController(ILogger<HomeController> logger, PostRepository postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
        }

        [HttpGet("/home/getposts")]
        public HttpResponseMessage Get(HttpRequest request)
        {
            var posts = _postRepository.GetAll().OrderBy(x => x.DateTimePosted);
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(posts)),
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }
    }
}