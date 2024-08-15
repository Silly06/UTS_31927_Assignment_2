using System.ComponentModel.DataAnnotations;

namespace PetPlayApp.Server.Models
{
	public class Notification
	{
		public Guid Id { get; set; }

		public Guid NotificationCreatorId { get; set; }
		public User? NotificationCreator { get; set; }

		public Guid PostId { get; set; }
		public Post? Post { get; set; }

		[Required]
		public User? Subject { get; set; }
		public Guid SubjectId { get; set; }

		public NotificationType? NotificationType { get; set; }
	}

	public enum NotificationType
	{
		PostLike,
		Comment,
		CommentLike,
		CommentReply
	}
}
