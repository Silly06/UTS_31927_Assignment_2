using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Controllers
{
    [Route("users")]
    public class UserController(IUserService userService) : Controller
    {
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
		
		[HttpGet("GetUserDetails")]
		public IActionResult GetUserDetails([FromQuery] Guid userId)
		{
			var userDetails = userService.GetUserDetails(userId);
			return Ok(userDetails);
		}

		[HttpPost("UpdateUserDetails")]
		public IActionResult UpdateUserDetails([FromBody] Guid userId, [FromBody] UserDetailsDto userDetails)
		{
			try
			{
				userService.UpdateUserDetails(userId, userDetails);
				return Ok("User details updated successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while updating user details: {ex.Message}");
			}
		}

		public class LoginRequest
		{
			public string Username { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
		}
	}
}