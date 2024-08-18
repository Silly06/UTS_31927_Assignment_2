using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Controllers;

[Route("users")]
public class UserController(IUserService userService) : Controller
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
        {
            return BadRequest("Username or password was empty or missing");
        }

        if (!userService.TryValidateUser(login.Username, login.Password, out var userId)) return Unauthorized();
        var picture = userService.GetUserPicture(userId);
        return Ok(new { UserId = userId, UserPfp = picture});
    }

    [HttpGet("GetUserDetails")]
    public IActionResult GetUserDetails([FromQuery] Guid userId)
    {
        var userDetails = userService.GetUserDetails(userId);
        return Ok(userDetails);
    }

    [HttpPost("UpdateUserDetails")]
    public async Task<IActionResult> UpdateUserDetailsAsync([FromBody] UserDetailsDto userDetails, [FromForm] IFormFile? profilePic)
    {
        try
        {
            byte[] imageData = [];
            if (profilePic != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePic.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }
            }
            userService.UpdateUserDetails(userDetails.UserId, userDetails.Username, userDetails.Email, userDetails.Age, userDetails.Bio, userDetails.Status, userDetails.Interest, imageData);
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

            userService.CreateUser(createUser.Username, createUser.Password, createUser.Email, createUser.Age, createUser.Bio, createUser.ProfilePicture);

            userService.TryValidateUser(createUser.Username, createUser.Password, out var userId);
            
            return Ok(new { UserId = userId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the user: {ex.Message}");
        }
    }
    
    [HttpGet("SearchUsers")]
    public IActionResult SearchUsers([FromQuery] Guid currentUserId, [FromQuery] string query)
    {
        var searchResults = userService.SearchUsers(currentUserId, query).ToList();
        
        if (searchResults.Count == 0)
        {
            return NotFound("No users found matching the search criteria.");
        }

        return Ok(searchResults);
    }

    [HttpGet("GetUserPicture")]
    public IActionResult GetUserPicture([FromQuery] Guid userId)
    {
        var picture = userService.GetUserPicture(userId);
        
        if (picture == null)
        {
            return NotFound("Profile picture not found.");
        }
        
        return Ok(picture);
    }

    [HttpPost("ResetPassword")]
    public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        if (string.IsNullOrWhiteSpace(resetPasswordDto.Email) || string.IsNullOrWhiteSpace(resetPasswordDto.OldPassword) || string.IsNullOrWhiteSpace(resetPasswordDto.NewPassword))
        {
            return BadRequest("Email, old password, or new password is missing.");
        }

        userService.ResetPassword(resetPasswordDto.Email, resetPasswordDto.OldPassword, resetPasswordDto.NewPassword);

        return Ok("Password reset successfully.");
    }

}
