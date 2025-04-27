using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using DatingApp.Api.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Repositories
{
    public class LikesRepository(ApplicationDbContext context) : ILikesRepository
    {
        private readonly ApplicationDbContext _context = context;

        public void AddLike(UserLike like)
        {
            _context.Likes.Add(like);
        }

        public void DeleteLike(UserLike like)
        {
            _context.Likes.Remove(like);
        }

        public async Task<IEnumerable<int>> GetCurrentUserIdsAsync(int currentUserId)
        {
            var LikedByUserIds = await _context.Likes
                    .Where(l => l.SourceUserId == currentUserId)
                    .Select(x => x.TargetUserId)
                    .ToListAsync();

            return LikedByUserIds;
        }

        public async Task<UserLike?> GetUserLikeAsync(int sourceUserId, int targetUserId)
        {
            var like = await _context.Likes
                    .SingleOrDefaultAsync(l => l.SourceUserId == sourceUserId
                        && l.TargetUserId == targetUserId);

            return like;
        }

        public async Task<IQueryable<ApplicationUser>> GetUserLikesAsync(string predicate, int userId)
        {
            var likes = _context.Likes
                            .AsQueryable();
            switch (predicate)
            {
                case "liked":
                    return likes
                            .Where(x => x.SourceUserId == userId)
                            .Select(x => x.TargetUser);
                case "likedBy":
                    return likes
                            .Where(x => x.TargetUserId == userId)
                            .Select(x => x.SourceUser);
                default:
                    var likeIds = await GetCurrentUserIdsAsync(userId);
                    return likes
                            .Where(x => x.TargetUserId == userId
                                && likeIds.Contains(x.SourceUserId))
                            .Select(x => x.SourceUser);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
