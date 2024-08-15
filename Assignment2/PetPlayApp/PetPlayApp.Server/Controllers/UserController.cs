using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;
using System.Text.Json;
using System.Net;
using PetPlayApp.Server.Db.Services;

namespace PetPlayApp.Server.Controllers
{
    [Route("posts")]
    public class UserController : Controller
    {
        private readonly Repository<User> userRepository;

        public UserController(ILogger<UserController> logger, RepositoryProvider repositoryProvider)
        {
            userRepository = repositoryProvider.GetRepository<User>();
        }

		[HttpGet("GetUsers")]
		public HttpResponseMessage GetUsers([FromBody] int page)
		{
			var userIds = userRepository.GetAll();

			var response = new HttpResponseMessage
			{
				Content = new StringContent(JsonSerializer.Serialize(userIds)),
				StatusCode = HttpStatusCode.OK
			};

			return response;
		}
	}
}