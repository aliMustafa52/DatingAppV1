namespace DatingApp.Api.Contracts.Authentication
{
    public record AuthResponse
    (
        int Id,
        string Username,
        string Token,
        int ExpiresIn,
        string? PhotoUrl
    );
}
