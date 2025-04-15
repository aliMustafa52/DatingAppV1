namespace DatingApp.Api.Contracts.Users
{
    public record PhotoResponse
    (
        int Id,
        string Url,
        bool IsMain,
        string? PublicId
    );
}
