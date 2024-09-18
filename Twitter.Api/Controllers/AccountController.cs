using Twitter.Application.Services;

namespace Twitter.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(UserManager<User> userManager,IAuthServices authServices) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new User
            { FirstName = registerDto.FirstName, LastName = registerDto.LastName, Email = registerDto.Email,UserName = registerDto.Email};
        var result = await userManager.CreateAsync(user, registerDto.Password);
        return Ok(result.Errors);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto loginDto,CancellationToken cancellationToken)
    {
        var authResult = 
            await authServices.GetTokenAsync(loginDto.Email,loginDto.Password,cancellationToken);
        
        return authResult is null ? BadRequest("Invalid Email or Password") : Ok(authResult);
    }
}