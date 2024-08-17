using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest("Username or password was empty or missing");
            }
            if (_userService.TryValidateUser(loginRequest.Username, loginRequest.Password, out var userId))
            {
                return Ok(new { UserId = userId });
            }
            return Unauthorized();
        }

        [HttpGet("GetUserDetails")]
        public IActionResult GetUserDetails([FromQuery] Guid userId)
        {
            var userDetails = _userService.GetUserDetails(userId);
            return Ok(userDetails);
        }

        [HttpPost("UpdateUserDetails")]
        public IActionResult UpdateUserDetails([FromBody] UpdateUserDetailsRequest request)
        {
            try
            {
                _userService.UpdateUserDetails(request.UserId, request.Username, request.Email, request.Age, request.Bio);
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

        public class UpdateUserDetailsRequest
        {
            public Guid UserId { get; set; }
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public int Age { get; set; }
            public string Bio { get; set; } = string.Empty;
        }
    }
}
