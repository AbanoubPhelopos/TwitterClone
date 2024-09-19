using Twitter.Contract.Users;

namespace Twitter.Contract.Comments;

public record CommentResponse(
    Guid Id,
    UserResponse Writer,
    string Content,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );