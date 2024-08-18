using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetPlayApp.Server.Models
{
	[PrimaryKey("CommentId", "UserId")]
    public class CommentLike
    {
        [ForeignKey("Comment")]
        public Guid CommentId { get; set; }
        public Comment? Comment { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
