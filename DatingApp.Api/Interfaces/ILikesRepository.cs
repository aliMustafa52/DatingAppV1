using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Entities;

namespace DatingApp.Api.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLikeAsync(int sourceUserId, int targetUserId);

        Task<IQueryable<ApplicationUser>> GetUserLikesAsync(string predicate, int userId);

        Task<IEnumerable<int>> GetCurrentUserIdsAsync(int currentUserId);

        void AddLike(UserLike like);

        void DeleteLike(UserLike like);

        Task<int> SaveChangesAsync();
    }
}
