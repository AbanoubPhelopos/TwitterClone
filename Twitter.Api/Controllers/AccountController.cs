using System.Linq.Expressions;
using Twitter.Contract.Models;
using Twitter.Contract.Posts;
using Twitter.Contract.Users;

namespace Twitter.Api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(ApplicationDbContext context, UserManager<User> userManager, IAuthServices authServices, IJwtProvider jwtProvider) : BaseController
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterDto request)
    {
        User newUser = new()
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
        };

        var result = await userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
        {
            throw new Exception("RegistrationFailed");
        }

        var (accessToken, expiresIn) = jwtProvider.GenerateToken(newUser);

        return Ok(new AuthResponse(newUser.Adapt<UserResponse>(), accessToken, accessToken, expiresIn));
    }

    [HttpPost("login", Name = "Login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
        {
            throw new Exception("InvalidCredential");
        }

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (result == false)
        {
            throw new Exception("InvalidCredential");
        }

        var (accessToken, expiresIn) = jwtProvider.GenerateToken(user);

        return Ok(new AuthResponse(user.Adapt<UserResponse>(), accessToken, accessToken, expiresIn));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<MeResponse>> GetMeAsync()
    {
        if (UserId is null)
        {
            return new MeResponse(null);
        }

        if (await userManager.FindByIdAsync(UserId.ToString()!) is not User user)
        {
            return new MeResponse(null);
        }

        return new MeResponse(user.Adapt<UserResponse>());
    }

    [Authorize]
    [HttpGet("me/posts")]
    public async Task<ActionResult<PagedListResponse<PostResponse>>> GetMyPostsAsync(int? page = 1, int? pageSize = 10)
    {
        if (UserId is null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        var query = context.Posts.AsNoTracking()
            .Where(p => p.AuthorId == UserId.Value);

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
            new UserResponse(p.Author.Id, p.Author.FirstName, p.Author.LastName, null),
            p.Title,
            p.Content,
            p.PostedAt,
            p.UpdatedAt,
            p.OriginalPost == null ? null :
            new PostResponse(
                p.OriginalPost!.Id,
                new UserResponse(p.OriginalPost!.Author.Id, p.OriginalPost!.Author.FirstName, p.OriginalPost!.Author.LastName, null),
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
}
