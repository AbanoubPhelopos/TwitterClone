namespace Twitter.Contract.Comments;

public record CreateCommentRequest(
    string Content
    );
    
public record UpdateCommentRequest(
    string Content
);