using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Twitter.Api.Authantication;

public class JwtProvider : IJwtProvider
{
    public (string token, int expiresIn) GenerateToken(User user)
    {
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RqqpETYPyJeeByw786ufuj333OdlcG0I"));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        
        var expiresIn = 30;

        var token = new JwtSecurityToken(
            issuer: "TwitterApp",
            audience: "TwitterApp users",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresIn),
            signingCredentials: signingCredentials
        );
        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn);
    }
}