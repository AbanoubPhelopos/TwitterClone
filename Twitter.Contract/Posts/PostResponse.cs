using Twitter.Contract.Users;

namespace Twitter.Contract.Posts;

public record PostResponse(
    Guid Id,
    UserResponse Author,
    string Title,
    string Content,
    DateTime PostedAt,
    DateTime UpdatedAt,
    PostResponse? OriginalPost,
    int LikesCount = 0,
    int CommentsCount = 0
    );

