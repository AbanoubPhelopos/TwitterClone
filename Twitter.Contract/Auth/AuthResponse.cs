namespace Twitter.Contract.Auth;

public record AuthResponse(
    string Id,
    string? Email,
    string FristName,
    string LastName,
    string Token,
    int ExpiresIn
);