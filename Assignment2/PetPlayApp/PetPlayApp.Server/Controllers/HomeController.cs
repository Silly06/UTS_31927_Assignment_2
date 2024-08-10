using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class HomeController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[HttpGet("/home/posts")]
		public IEnumerable<Post> Get(HttpRequest request)
		{
			return new List<Post>();
;		}
	}
}
