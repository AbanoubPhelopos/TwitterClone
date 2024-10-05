namespace Twitter.Contract.Notification;

public record NotificationResponse(
    Guid Id,
    DateTime CreatedAt,
    bool Read ,
    string Message ,
    string Type ,
    Guid? RelatedEntityId ,
    string RelatedEntityType ,
    bool IsActionable 
    );