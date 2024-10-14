using Twitter.Contract.Users;

namespace Twitter.Api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(UserManager<User> userManager, IAuthServices authServices, IJwtProvider jwtProvider) : BaseController
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

}