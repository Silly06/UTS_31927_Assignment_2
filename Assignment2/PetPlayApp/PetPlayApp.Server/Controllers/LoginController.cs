using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Db.Services;

namespace PetPlayApp.Server.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly UserService userService;

        public LoginController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public IActionResult Login([FromForm] string? username, [FromForm] string? password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Username or password was empty or missing");
            }
            if (userService.TryValidateUser(username, password, out var userId))
            {
                return Ok(new { UserId = userId });
            }
            return Unauthorized();
        }
    }
}
