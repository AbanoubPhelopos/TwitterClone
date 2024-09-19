namespace Twitter.Application.Services;
public class AuthServices(UserManager<User> userManager, IJwtProvider jwtProvider) : IAuthServices
{
    public async Task<AuthResponse?> GetTokenAsync(string email, string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

        var isValidPassword = await userManager.CheckPasswordAsync(user, password);

        if (!isValidPassword)
            return null;

        var (token, expiresIn) = jwtProvider.GenerateToken(user);

        throw new Exception("TODO");
    }
}