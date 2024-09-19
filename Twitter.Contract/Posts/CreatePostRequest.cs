namespace Twitter.Contract.Post;

public record CreatePostRequest(
    string Title,
    string Content
    );


public record UpdatePostRequest(
    string Title,
    string Content
    );
