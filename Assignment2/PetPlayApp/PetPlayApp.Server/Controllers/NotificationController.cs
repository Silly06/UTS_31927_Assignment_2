using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services;
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
        public ActionResult<IEnumerable<Guid>> GetRecentNotifications(Guid userId)
        {
            var notificationIds = _notificationService.GetRecentNotifications(userId);
            if (notificationIds == null || notificationIds.Count == 0)
            {
                return NotFound();
            }
            return Ok(notificationIds);
        }

        [HttpGet("GetNotificationDetails")]
        public IActionResult GetNotificationDetails([FromQuery] Guid postid)
        {
            var notification = _notificationService.GetNotification(postid);
            return Ok(notification);
        }
    }
}
