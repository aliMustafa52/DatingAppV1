namespace DatingApp.Api.Contracts.Authentication
{
    public record AuthResponse
    (
        int Id,
        string Username,
        string KnownAs,
        string Token,
        int ExpiresIn,
        string? PhotoUrl
    );
}
