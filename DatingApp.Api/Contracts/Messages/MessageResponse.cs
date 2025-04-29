namespace DatingApp.Api.Contracts.Messages
{
    public record MessageResponse
    (
        int Id,
        int SenderId,
        string SenderUsername,
        string SenderPhotoUrl,
        int RecipientId,
        string RecipientUsername,
        string RecipientPhotoUrl,
        string Content,
        DateTime SentOn,
        DateTime? ReadOn
    );
}
