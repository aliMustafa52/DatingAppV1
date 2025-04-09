namespace DatingApp.Api.Contracts.Authentication
{
    public record LoginRequest
    (
        string Username,
        string Password
    );
}
