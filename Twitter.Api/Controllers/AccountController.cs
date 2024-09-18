namespace Twitter.Api.Controllers;

[ApiController]
[Route("api/profile")]
public class AccountController(UserManager<User> userManager) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new User
            { FirstName = registerDto.FirstName, LastName = registerDto.LastName, Email = registerDto.Email,UserName = registerDto.Email};
        var result = await userManager.CreateAsync(user, registerDto.Password);
        return Ok(result.Errors);
    }
}