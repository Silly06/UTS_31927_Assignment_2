using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginRequest loginRequest)
		{
			if (string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
			{
				return BadRequest("Username or password was empty or missing");
			}
			if (userService.TryValidateUser(loginRequest.Username, loginRequest.Password, out var userId))
			{
				return Ok(new { UserId = userId });
			}
			return Unauthorized();
		}
		
		[HttpGet("{userId:guid}/details")]
		public IActionResult GetUserDetails(Guid userId)
		{
			var userDetails = userService.GetUserDetails(userId);
			return Ok(userDetails);
		}

		public class LoginRequest
		{
			public string Username { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
		}
	}
}