using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Services.Abstractions;

#nullable enable

namespace PetPlayApp.Server.Controllers
{
	[Route("comments")]
	public class CommentController(ICommentService commentService) : Controller
	{
		[HttpPost("AddComment")]
		public IActionResult AddComment([FromForm] Guid postId, [FromForm] Guid userId, [FromForm] string? content)
		{
			try
			{
				if (postId == Guid.Empty || userId == Guid.Empty || string.IsNullOrWhiteSpace(content))
				{
					return BadRequest("Invalid request data.");
				}

				commentService.AddComment(postId, userId, content);

				return Ok("Comment created successfully.");
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while processing your request.");
			}
		}

		[HttpGet("GetComments")]
		public IActionResult GetComments([FromQuery] GetCommentRequest request)
		{
			try
			{
				var comment = commentService.GetCommentsForPost(request.PostId);
				return Ok(comment);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("LikeComment")]
		public IActionResult LikePost([FromBody] Guid commentId, [FromBody] Guid userId)
		{
			try
			{
				commentService.UnlikeComment(commentId, userId);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("UnlikeComment")]
		public IActionResult UnlikePost([FromBody] Guid commentId, [FromBody] Guid userId)
		{
			try
			{
				commentService.UnlikeComment(commentId, userId);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
	public class GetCommentRequest
	{
		public Guid PostId { get; set; }
	}

	public class AddCommentRequest
	{
		public Guid PostId { get; set; }
		public Guid UserId { get; set; }
		public string? Content { get; set; }
	}
}