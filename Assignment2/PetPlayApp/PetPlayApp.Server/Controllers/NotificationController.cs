using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Controllers
{
	[Route("notifications")]
	public class NotificationController : Controller
	{
		private readonly INotificationService _notificationService;

		public NotificationController(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		[HttpGet("GetRecentNotifications")]
		public ActionResult<IEnumerable<Guid>> GetRecentNotifications([FromQuery] Guid userId)
		{
			var notifications = _notificationService.GetRecentNotifications(userId);
			if (notifications == null || notifications.Count == 0)
			{
				return NoContent();
			}
			return Ok(notifications);
		}
	}
}
