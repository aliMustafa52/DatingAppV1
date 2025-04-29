namespace DatingApp.Api.Contracts.Messages
{
    public record MessageRequest
    (
        string RecipientUsername,
        string Content
    );
}
