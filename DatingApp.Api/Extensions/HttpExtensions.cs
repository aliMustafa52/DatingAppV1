using DatingApp.Api.Abstractions;
using System.Text.Json;

namespace DatingApp.Api.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader<T>(this HttpResponse response, PaginatedList<T> data)
        {
            var paginationHeader = new PaginationHeader(data.PageNumber,data.TotalCount, data.PageSize, data.TotalPages, data.HasNextPage, data.HasPreviousPage);

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            response.Headers
                .Append("Pagination", JsonSerializer.Serialize(paginationHeader, jsonOptions));

            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
