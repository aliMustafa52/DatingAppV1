namespace DatingApp.Api.Contracts.Users
{
    public record UserResponse
    (
        int Id,
        string UserName,
        int Age,
        string PhotoUrl,
        string KnownAs,
        DateTime Created,
        DateTime LastActive,
        string Gender,
        string? Introduction,
        string? Interests,
        string? LookingFor,
        string City,
        string Country,
        IEnumerable<PhotoResponse> Photos
    );
}
