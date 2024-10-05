namespace Twitter.Contract.Posts;

public record CreatePostRequest(
    string Title,
    string Content,
    Guid? OriginalPostId
    );


public record UpdatePostRequest(
    string Title,
    string Content
    );
