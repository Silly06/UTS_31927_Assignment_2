using System.ComponentModel.DataAnnotations;

namespace PetPlayApp.Server.Models
{
	public class Notification
	{
		public Guid Id { get; set; }

		public Guid CreatorId { get; set; }
		public User? Creator { get; set; }

		public Guid PostId { get; set; }
		public Post? Post { get; set; }

		[Required]
		public User? Subject { get; set; }
		public Guid SubjectId { get; set; }

		public DateTime Timestamp { get; set; }

		public NotificationType? NotificationType { get; set; }
	}

	public enum NotificationType
	{
		PostLike,
		Comment,
		CommentLike,
	}
}
