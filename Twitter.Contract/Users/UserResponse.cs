namespace Twitter.Contract.Users;
public record UserResponse(
    Guid Id,
    //string UserName,
    string FirstName,
    string LastName,
    string? ProfilePicture
);