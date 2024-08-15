using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

[Route("notifications")]
public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("user")]
    public ActionResult<IEnumerable<Notification>> GetNotificationsForUser(Guid userId)
    {
        var notifications = _notificationService.GetNotificationsForUser(userId);
        if (notifications == null || !notifications.Any())
        {
            return NotFound();
        }
        return Ok(notifications.OrderByDescending(n => n.Timestamp));
    }
}
