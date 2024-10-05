using Twitter.Contract.Models;
using Twitter.Contract.Notification;

namespace Twitter.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController(ApplicationDbContext context) : BaseController
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<NotificationResponse>>> GetNotificationsAsync(int? page = 1, int? pageSize = 10)
        {
            var userId = UserId!.Value;

            var query = context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .AsNoTracking();

            var total = await query.CountAsync();
            var offset = (page!.Value - 1) * pageSize!.Value;
            var limit = pageSize!.Value;
            var pages = (int)Math.Ceiling((double)total / pageSize!.Value);

            query = query.Skip(offset).Take(limit);

            var notifications = await query.Select(n => new NotificationResponse(
                n.Id,
                n.CreatedAt,
                n.Read,
                n.Message,
                n.Type,
                n.RelatedEntityId,
                n.RelatedEntityType,
                n.IsActionable
            )).ToListAsync();

            return Ok(new PagedListResponse<NotificationResponse>(notifications, page.Value, pages));
        }

        [HttpPut("{notificationId:guid}/mark-read")]
        [Authorize]
        public async Task<IActionResult> MarkNotificationAsReadAsync(Guid notificationId)
        {
            var userId = UserId!.Value;
            var notification = await context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification == null)
            {
                return NotFound("Notification not found.");
            }

            notification.Read = true;
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
