using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetPlayApp.Server.Models
{
	[PrimaryKey("PostId", "UserId")]
	public class Like
	{
		[ForeignKey("Post")]
		public Guid PostId { get; set; }
		public Post? Post { get; set; }

		[ForeignKey("User")]
		public Guid UserId { get; set; }
		public User? User { get; set; }
	}
}
