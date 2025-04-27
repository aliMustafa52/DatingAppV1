using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Entities;
using DatingApp.Api.Errors;
using DatingApp.Api.Interfaces;
using DatingApp.Api.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Services.LikesService
{
    public class LikesService(ILikesRepository likesRepository) : ILikesService
    {
        private readonly ILikesRepository _likesRepository = likesRepository;

        public async Task<Result> ToggleLikeAsync(int sourceUserId,int targetUserId)
        {
            if (sourceUserId == targetUserId)
                return Result.Failure(LikeErrors.CannotLikeYourself);

            var existingLike = await _likesRepository.GetUserLikeAsync(sourceUserId, targetUserId);
            if(existingLike is null)
            {
                var like = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };

                _likesRepository.AddLike(like);
            }
            else
            {
                _likesRepository.DeleteLike(existingLike);
            }

            var updated = await _likesRepository.SaveChangesAsync();
            if (updated == 0)
                return Result.Failure<UserResponse>(UserErrors.FailedToSaveChanges);

            return Result.Success();

        }

        public async Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int userId)
        {
            return await _likesRepository.GetCurrentUserIdsAsync(userId);
        }

        public async Task<PaginatedList<UserResponse>> GetUserLikesAsync(RequestLikesFilters requestLikesFilters, CancellationToken cancellationToken = default)
        {
            var userLikes =await _likesRepository.GetUserLikesAsync(requestLikesFilters.Predicate, requestLikesFilters.UserId);

            var userResponses = userLikes.ProjectToType<UserResponse>();

            var response = await PaginatedList<UserResponse>.CreateAsync(userResponses,
                requestLikesFilters.PageNumber, requestLikesFilters.PageSize, cancellationToken);

            return response;
        }
    }
}
