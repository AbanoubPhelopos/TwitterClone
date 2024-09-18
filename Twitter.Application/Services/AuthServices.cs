namespace Twitter.Application.Services;

public class AuthServices(UserManager<User> userManager) : IAuthServices
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

        //add jwt config logic

        return new AuthResponse(user.Id.ToString(), user.Email, user.FirstName, user.LastName, "fskh", 3600);
    }
}