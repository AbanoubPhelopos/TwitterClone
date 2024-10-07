namespace Twitter.Application.Models;

public class Follow
{
    public Guid FollowerId { get; init; }
    public Guid FolloweeId { get; init; }
}