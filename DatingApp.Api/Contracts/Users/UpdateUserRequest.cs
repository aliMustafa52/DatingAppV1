namespace DatingApp.Api.Contracts.Users
{
    public record UpdateUserRequest
    (
        string Introduction,
        string Interests,
        string LookingFor,
        string City,
        string Country
    );
}
