namespace DatingApp.Api.Contracts.Common
{
    public record RequestFilters
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        //public string? SearchValue { get; init; }

        public string? Gender { get; init; }
        public string? CurrentUsername { get; set; }
        public int MinAge { get; init; } = 18;
        public int MaxAge { get; set; } = 100;

        public string OrderBy { get; init; } = "lastActive";
    }
}
