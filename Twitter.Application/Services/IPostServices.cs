using Twitter.Contract.Post;

namespace Twitter.Application.Services;

public interface IPostServices
{
    Task<User> GetUserByName(string name);
    //Task<PostResponse> SharePost(Post )
}