namespace DatingApp.Api.Contracts.Authentication
{
    public record RegisterRequest
    (
        string Username,
        string Password,
        string KnownAs,
        string Gender,
        DateOnly DateOfBirth,
        string City,
        string Country
    );
}
