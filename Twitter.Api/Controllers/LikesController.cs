using System.Linq.Expressions;
using Twitter.Contract.Models;
using Twitter.Contract.Users;

namespace Twitter.Api.Controllers;

[ApiController]
[Route("api/posts/{postId:guid}/likes")]
public class LikesController(ApplicationDbContext context,UserManager<User> userManager) : BaseController
{
    // Toggle like/unlike a post
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> ToggleLikeAsync(Guid postId)
    {
        var userId = UserId!.Value;

        if (await context.Posts
                .Include(p=>p.Likes.Where(l=>l.LikerId == userId))
                .FirstOrDefaultAsync(p => p.Id == postId) is not Post post)
        {
            throw new Exception("PostNotFound");
        }

        // Check if the user has already liked the post
        var existingLike = post.Likes.FirstOrDefault(l => l.LikerId == userId);
        
        if (existingLike != null)
        {
            post.Likes.Remove(existingLike);
            var notification = await context.Notifications.FirstOrDefaultAsync(n => n.RelatedEntityId == postId);
            if(notification is not null)
                context.Notifications.Remove(notification);
            await context.SaveChangesAsync();
            return Ok("PostUnliked");
        }
        else
        {
            // If no like exists, add a new like
            var newLike = new Like
            {
                LikerId = userId,
              
            };
            var actionUser = await userManager.FindByIdAsync(userId.ToString());
            
            var newNotification = new Notification
            {
                UserId = post.AuthorId,
                CreatedAt = DateTime.UtcNow,
                Message = $"{actionUser!.FirstName} {actionUser!.LastName} has Liked your post",
                Type = "New Like",
                RelatedEntityId = postId,
                RelatedEntityType = "Post"
            };
            context.Notifications.Add(newNotification);
            post.Likes.Add(newLike);
            await context.SaveChangesAsync();
            return Ok("PostLiked");
        }
    }

    // Get list of users who liked the post
    [HttpGet(Name = "GetLikes")]
    public async Task<ActionResult<PagedListResponse<UserResponse>>> GetLikesAsync(Guid postId, int? page = 1, int? pageSize = 10)
    {
        var query = context.Likes
            .Where(l => l.PostId == postId)
            .Include(l => l.Liker)
            .AsNoTracking();

        query = query.OrderBy(l => l.LikerId);

        var total = await query.CountAsync();
        var offset = (page!.Value - 1) * pageSize!.Value;
        var limit = pageSize!.Value;
        var pages = (int)Math.Ceiling((double)total / pageSize!.Value);

        query = query.Skip(offset).Take(limit);

        var items = await query.Select(SelectUser()).ToListAsync();

        return Ok(new PagedListResponse<UserResponse>(items, page!.Value, pages));
    }

    private static Expression<Func<Like, UserResponse>> SelectUser()
    {
        return l => new UserResponse(
            l.Liker.Id,
            l.Liker.UserName!,
            l.Liker.FirstName,
            l.Liker.LastName,
            null
        );
    }
}
