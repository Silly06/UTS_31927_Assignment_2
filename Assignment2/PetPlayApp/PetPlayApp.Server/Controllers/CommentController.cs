using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Controllers
{
	[Route("comments")]
	public class CommentController : Controller
	{
		private readonly ICommentService commentService;

		public CommentController(ICommentService commentService)
		{
			this.commentService = commentService;
		}

		[HttpPost("AddComment")]
		public IActionResult AddComment([FromBody] AddCommentRequest request)
		{
			try
			{
				var comment = commentService.AddComment(request.PostId, request.UserId, request.Content);

				return Ok(comment);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("GetComments")]
		public IActionResult LikeComment([FromBody] GetCommentRequest request)
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