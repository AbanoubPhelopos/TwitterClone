using Twitter.Contract.Users;

namespace Twitter.Contract.Auth;

public record AuthResponse(
    UserResponse Me,
    string AccessToken,
    string RefreshToken,
    int ExpiresIn
);