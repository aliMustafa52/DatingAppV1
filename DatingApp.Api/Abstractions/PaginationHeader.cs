namespace DatingApp.Api.Abstractions
{
    public class PaginationHeader(int pageNumber,int totalCount, int pageSize, int totalPages, bool hasPreviousPage, bool hasNextPage)
    {
        public int PageNumber { get; set; } = pageNumber;
        public int TotalCount { get; set; } = totalCount;
        public int PageSize { get; set; } = pageSize;
        public int TotalPages { get; set; } = totalPages;
        public bool HasPreviousPage { get; set; } = hasPreviousPage;
        public bool HasNextPage { get; set; } = hasNextPage;
    }
}
