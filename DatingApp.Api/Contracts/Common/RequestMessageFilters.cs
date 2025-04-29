namespace DatingApp.Api.Contracts.Common
{
    public record RequestMessageFilters : RequestPaginationFilters
    {
        public string? Username {  get; set; }
        public string Container { get; set; } = "Unread";
    }
}
