using Twitter.Contract.Users;

namespace Twitter.Contract.Likes;
public record LikeResponse(
    UserResponse Liker,
DateTime CreatedAt,
DateTime UpdatedAt
);