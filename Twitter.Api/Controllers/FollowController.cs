namespace Twitter.Api.Controllers
{
    [ApiController]
    [Route("api/users/{followeeId:guid}/follow")]
    [Authorize]
    public class FollowController(ApplicationDbContext context, UserManager<User> userManager)
        : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> FollowAsync(Guid followeeId)
        {
            var userId = UserId!.Value;

            var followee = await userManager.FindByIdAsync(followeeId.ToString());
            if (followee == null)
            {
                return NotFound("User not found.");
            }

            var existingFollow = await context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == userId && f.FolloweeId == followeeId);

            if (existingFollow != null)
            {
                context.Follows.Remove(existingFollow);
                await context.SaveChangesAsync();
                return Ok("Unfollowed successfully.");
            }
            else
            {
                var follow = new Follow
                {
                    FolloweeId = followeeId,
                    FollowerId = userId
                };

                context.Follows.Add(follow);

                var actionUser = await userManager.FindByIdAsync(userId.ToString());

                var newNotification = new Notification
                {
                    UserId = followeeId,
                    CreatedAt = DateTime.UtcNow,
                    Message = $"{actionUser!.FirstName} {actionUser!.LastName} has followed you",
                    Type = "New Follow",
                    RelatedEntityId = userId,
                    RelatedEntityType = "User"
                };
                context.Notifications.Add(newNotification);

                await context.SaveChangesAsync();
                return Ok("Followed successfully.");
            }
        }
    }
}