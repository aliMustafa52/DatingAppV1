namespace DatingApp.Api.Contracts.Authentication
{
    public record AuthResponse
    (
        int Id,
        string Username,
        int Age,
        string Token,
        int ExpiresIn
    );
}
