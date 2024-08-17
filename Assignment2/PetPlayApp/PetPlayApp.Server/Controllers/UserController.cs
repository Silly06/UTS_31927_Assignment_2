using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Dto;
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
        public IActionResult Login([FromBody] LoginDto login)
        {
            if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest("Username or password was empty or missing");
            }
            if (_userService.TryValidateUser(login.Username, login.Password, out var userId))
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
        public IActionResult UpdateUserDetails([FromBody] UserDetailsDto userDetails)
        {
            try
            {
                _userService.UpdateUserDetails(userDetails.UserId, userDetails.Username, userDetails.Email, userDetails.Age, userDetails.Bio);
                return Ok("User details updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating user details: {ex.Message}");
            }
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] CreateUserDto createUser)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createUser.Username) || string.IsNullOrWhiteSpace(createUser.Password))
                {
                    return BadRequest("Username or password cannot be empty.");
                }

                _userService.CreateUser(createUser.Username, createUser.Password, createUser.Email, createUser.Age, createUser.Bio);

                _userService.TryValidateUser(createUser.Username, createUser.Password, out var userId);
                
                return Ok(new { UserId = userId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the user: {ex.Message}");
            }
        }
    }
}
