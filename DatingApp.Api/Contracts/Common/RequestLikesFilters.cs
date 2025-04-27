namespace DatingApp.Api.Contracts.Common
{
    public record RequestLikesFilters : RequestPaginationFilters
    {
        public int UserId { get; set; }
        public required string Predicate { get; set; } = "liked";
    }
}
