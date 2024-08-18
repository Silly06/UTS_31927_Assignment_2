using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

#nullable enable

namespace PetPlayApp.Server.Controllers
{
	[Route("posts")]
	public class PostsController(IPostService postService) : Controller
	{
		[HttpGet("GetRecentPosts")]
		public IActionResult GetRecentPosts([FromQuery] int page)
		{
			var postsIds = postService.GetRecentPosts(page);
			return Ok(postsIds);
		}

		[HttpGet("GetUserPosts")]
		public IActionResult GetUserPosts([FromQuery] int page, [FromQuery] Guid userid)
		{
			var postsIds = postService.GetUserPosts(page, userid);
			return Ok(postsIds);
		}

		[HttpGet("GetPostDetails")]
		public IActionResult GetPostDetails([FromQuery] Guid postid, [FromQuery] Guid userId)
		{
			var post = postService.GetPost(postid)!;
			return Ok(new PostDetailsDto
			{
				PostId = post.Id,
				LikesCount = post.Likes.Count,
				LikedByUser = post.Likes.Select(u => u.Id).Contains(userId),
				ImageData = post.ImageData,
				Description = post.Description
			});
		}

		[HttpPost("NewPost")]
		public async Task<IActionResult> NewPost([FromForm] IFormFile? image, [FromForm] string? description, [FromForm] Guid postCreatorId)
		{
			if (image == null || image.Length == 0)
				return BadRequest("Image is required");
			if (description == null)
				return BadRequest("Description is required");

			byte[] imageData;
			using (var memoryStream = new MemoryStream())
			{
				await image.CopyToAsync(memoryStream);
				imageData = memoryStream.ToArray();
			}

			var post = new Post
			{
				Id = Guid.NewGuid(),
				DateTimePosted = DateTime.Now,
				PostCreatorId = postCreatorId,
				Description = description,
				ImageData = imageData
			};

			postService.AddPost(post);

			return Ok(post);
		}

		[HttpPost("LikePost")]
		public IActionResult LikePost([FromBody] LikePostDto likePost)
		{
			try
			{
				var result = postService.LikePost(likePost.PostId, likePost.UserId);
				if (result == null) return BadRequest("Error processing like");
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("UnlikePost")]
		public IActionResult UnlikePost([FromBody] LikePostDto likePost)
		{
			try
			{
				var result = postService.UnlikePost(likePost.PostId, likePost.UserId);
				if (result == null) return BadRequest("Error processing unlike");
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}