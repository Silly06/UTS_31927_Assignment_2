#nullable enable
namespace PetPlayApp.Server.Dto
{
	public class NotificationDto
	{
		public Guid PostId { get; set; }
		public string? Content { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
