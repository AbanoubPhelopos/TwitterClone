﻿using System.Collections.Immutable;
using System.Linq.Expressions;
using Twitter.Contract.Models;
using Twitter.Contract.Post;
using Twitter.Contract.Posts;
using Twitter.Contract.Users;

namespace Twitter.Api.Controllers;


[ApiController]
[Route("api/posts")]
public class PostController(ApplicationDbContext context) : BaseController
{
    private readonly ApplicationDbContext _context = context;
    
    [HttpPost(Name = "CreatePost")]
    [Authorize]
    public async Task<ActionResult<PostResponse>> CreatePostAsync([FromBody] CreatePostRequest request)
    {
        var now = DateTime.UtcNow;

        var newPost = new Post
        {
            AuthorId = UserId!.Value,
            PostedAt = now,
            UpdatedAt = now,
            Title = request.Title, 
            Content = request.Content,
        };

        _context.Posts.Add(newPost);
        await _context.SaveChangesAsync();

        return Ok(newPost.Adapt<PostResponse>());
    }
    
    [HttpPut("{postId:guid}",Name = "UpdatePost")]
    [Authorize]
    public async Task<ActionResult<PostResponse>> UpdatePostAsync([FromBody] UpdatePostRequest request, Guid postId)
    {
        if (await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId) is not Post post)
        {
            throw new Exception("PostNotFound");
        }

        if (post.AuthorId != UserId)
        {
            throw new Exception("UnauthorizedException");
        }
        
        var now = DateTime.UtcNow;

        post.Content = request.Content;
        post.Title = request.Title;
        post.UpdatedAt = now;
        
        await _context.SaveChangesAsync();

        return Ok(post.Adapt<PostResponse>());
    }

    [HttpGet(Name = "Posts")]
    public async Task<ActionResult<PagedListResponse<PostResponse>>> GetPostsAsync(string? search, int? page = 1, int? pageSize = 10)
    {
        var query = _context.Posts.AsNoTracking();

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(p => p.Title.ToLower().Contains(search) || p.Content.ToLower().Contains(search));
        }

        query = query.OrderBy(p => p.PostedAt).ThenBy(p => p.Id);

        var total = await query.CountAsync();
        var offset = (page!.Value - 1) * pageSize!.Value;
        var limit = pageSize!.Value;
        var pages = (int)Math.Ceiling((double)total / pageSize!.Value);


        query = query.Skip(offset).Take(limit);

        var items = await query.Select(SelectPost()).ToListAsync();

        return Ok(new PagedListResponse<PostResponse>(items, page!.Value, pages));
    }

    private static Expression<Func<Post, PostResponse>> SelectPost()
    {
        return p => new PostResponse(
            p.Id,
            new UserResponse(p.Author.Id, p.Author.UserName!, p.Author.FirstName, p.Author.LastName, null),
            p.Title,
            p.Content,
            p.PostedAt,
            p.UpdatedAt, p.OriginalPost ==null ? null :
            new PostResponse(
                p.OriginalPost!.Id,
                new UserResponse(p.OriginalPost!.Author.Id, p.OriginalPost!.Author.UserName!, p.OriginalPost!.Author.FirstName, p.OriginalPost!.Author.LastName, null),
                p.OriginalPost!.Title,
                p.OriginalPost!.Content,
                p.OriginalPost!.PostedAt,
                p.OriginalPost!.UpdatedAt,
                null,
                0,
                0
            ),
            0,
            0
        );
    }

    [HttpGet("feed",Name = "Feed")]
    public async Task<ActionResult> GetFeedAsync(string? search)
    {
        throw new NotImplementedException();
    }
}