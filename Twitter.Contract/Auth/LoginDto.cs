namespace Twitter.Contract.Auth;

public record LoginDto(
    string Email,
    string Password
    );