using Microsoft.AspNetCore.Mvc;

namespace PetPlayApp.Server.Controllers
{
	public class HomeController : Controller
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

		[HttpGet("/home/getposts")]
		public IEnumerable<Post> Get(HttpRequest request)
		{
			return new List<Post>();
;		}
	}
}
