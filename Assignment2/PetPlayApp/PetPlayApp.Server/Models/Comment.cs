using System.ComponentModel.DataAnnotations;

namespace PetPlayApp.Server.Models
{
	public class Comment
    {
        public Guid Id { get; set; }

        [Required]
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid PostId { get; set; }
        public Post? Post { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

		public List<User> Likes { get; init; } = [];
	}
}
