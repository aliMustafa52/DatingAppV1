namespace DatingApp.Api.Contracts.Authentication
{
    public record RegisterRequest
    (
        string Username,
        string Password
    );
}
