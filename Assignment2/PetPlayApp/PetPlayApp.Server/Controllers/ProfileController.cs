using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;
using System.Text.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PetPlayApp.Server.Controllers
{
    [Route("posts")]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly UserRepository _userRepository;

        public ProfileController(ILogger<ProfileController> logger, UserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

		[HttpGet("GetUsers")]
		public HttpResponseMessage GetUsers([FromBody] int page)
		{
			var userIds = _userRepository.GetAll()
				.ToList();

			var response = new HttpResponseMessage
			{
				Content = new StringContent(JsonSerializer.Serialize(userIds)),
				StatusCode = HttpStatusCode.OK
			};

			return response;
		}
	}
}