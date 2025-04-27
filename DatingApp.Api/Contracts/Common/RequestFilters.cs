namespace DatingApp.Api.Contracts.Common
{
    public record RequestFilters : RequestPaginationFilters
    {
        public string? Gender { get; init; }
        public string? CurrentUsername { get; set; }
        public int MinAge { get; init; } = 18;
        public int MaxAge { get; set; } = 100;

        public string OrderBy { get; init; } = "lastActive";
    }
}
