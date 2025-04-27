using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Users;

namespace DatingApp.Api.Services.LikesService
{
    public interface ILikesService
    {
        Task<Result> ToggleLikeAsync(int sourceUserId, int targetUserId);

        Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int userId);

        Task<PaginatedList<UserResponse>> GetUserLikesAsync(RequestLikesFilters requestLikesFilters, CancellationToken cancellationToken = default);
    }
}
