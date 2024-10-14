using System.Linq.Expressions;
using Twitter.Contract.Comments;
using Twitter.Contract.Models;
using Twitter.Contract.Posts;
using Twitter.Contract.Users;

namespace Twitter.Api.Controllers;

[ApiController]
[Route("api/posts/{postId:guid}/comments")]
public class CommentsController(ApplicationDbContext context,UserManager<User> userManager) : BaseController
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CommentResponse>> AddCommentAsync(CreateCommentRequest request ,Guid postId)
    {
        if (await context.Posts.FirstOrDefaultAsync(p => p.Id == postId) is not Post post)
        {
            throw new Exception("PostNotFound");
        }
        
        var now = DateTime.UtcNow;
        
        var newComment = new Comment()
        {
            WriterId = UserId!.Value,
            CreatedAt = now,
            UpdatedAt = now,
            Content = request.Content,
        };
        
        var actionUser = await userManager.FindByIdAsync(UserId.ToString()!);
            
        var newNotification = new Notification
        {
            UserId = post.AuthorId,
            CreatedAt = DateTime.UtcNow,
            Message = $"{actionUser!.FirstName} {actionUser!.LastName} has Commented at your post",
            Type = "New Comment",
            RelatedEntityId = postId,
            RelatedEntityType = "Post"
        };
        context.Notifications.Add(newNotification);
        
        post.Comments.Add(newComment);
        
        await context.SaveChangesAsync();

        return Ok(newComment.Adapt<CommentResponse>());
    }
    
    [HttpPut("{commentId:guid}",Name = "UpdateComment")]
    [Authorize]
    public async Task<ActionResult<PostResponse>> UpdateCommentAsync([FromBody] UpdateCommentRequest request,Guid postId, Guid commentId)
    {
        if (await context.Posts.FirstOrDefaultAsync(p => p.Id == postId) is not Post post)
        {
            throw new Exception("PostNotFound");
        }
        
        if (await context.Comments.Include(c=>c.Writer).FirstOrDefaultAsync(c => c.PostId == postId && c.Id == commentId) is not Comment comment)
        {
            throw new Exception("CommentNotFound");
        }

        if (comment.WriterId != UserId)
        {
            throw new Exception("UnauthorizedException");
        }
        
        var now = DateTime.UtcNow;

        comment.Content = request.Content;
        comment.UpdatedAt = now;
        
        await context.SaveChangesAsync();

        return Ok(comment.Adapt<CommentResponse>());
    }
    
    
    [HttpGet(Name = "Comments")]
    public async Task<ActionResult<PagedListResponse<CommentResponse>>> GetCommentsAsync(Guid postId ,int? page = 1, int? pageSize = 10)
    {
        var query = context.Comments.Where(c=>c.PostId==postId).AsNoTracking();

        query = query.OrderBy(c => c.CreatedAt).ThenBy(c => c.Id);

        var total = await query.CountAsync();
        var offset = (page!.Value - 1) * pageSize!.Value;
        var limit = pageSize!.Value;
        var pages = (int)Math.Ceiling((double)total / pageSize!.Value);

        query = query.Skip(offset).Take(limit);

        var items = await query.Select(SelectComment()).ToListAsync();

        return Ok(new PagedListResponse<CommentResponse>(items, page!.Value, pages));
    }
    private static Expression<Func<Comment, CommentResponse>> SelectComment()
    {
        return c => new CommentResponse(
            c.Id,
            new UserResponse(c.Writer.Id, c.Writer.FirstName, c.Writer.LastName, null),
            c.Content,
            c.CreatedAt,
            c.UpdatedAt
        );
    }
    
    [HttpDelete("{commentId:guid}", Name = "DeleteComment")]
    [Authorize]
    public async Task<IActionResult> DeleteCommentAsync(Guid postId, Guid commentId)
    {
        if (await context.Posts.FirstOrDefaultAsync(p => p.Id == postId) is not Post post)
        {
            return NotFound(new { Message = "Post not found" });
        }

        if (await context.Comments.FirstOrDefaultAsync(c => c.PostId == postId && c.Id == commentId) is not Comment comment)
        {
            return NotFound(new { Message = "Comment not found" });
        }

        if (comment.WriterId != UserId)
        {
            return Forbid();
        }

        context.Comments.Remove(comment);
        await context.SaveChangesAsync();

        return NoContent();
    }

}