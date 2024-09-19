using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Twitter.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    public Guid? UserId => User.FindFirstValue(JwtRegisteredClaimNames.Sub) is string id ? Guid.Parse(id) : null;
}
