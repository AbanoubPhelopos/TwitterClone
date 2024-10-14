public class Follow
{
    public Guid FollowerId { get; init; }
    public User Follower { get; set; } = default!;
    
    public Guid FolloweeId { get; init; }
    public User Followee { get; set; } = default!;
}