namespace DatingApp.Api.Contracts.Common
{
    public record RequestPaginationFilters
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
