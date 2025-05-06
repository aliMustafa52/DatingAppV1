namespace DatingApp.Api.Contracts.Users
{
    public record UserWithRolesResponse
    (
        int Id,
        string Username,
        IEnumerable<string?> Roles  
    );
}
