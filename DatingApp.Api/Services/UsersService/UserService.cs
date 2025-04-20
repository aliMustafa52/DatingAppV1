using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Entities;
using DatingApp.Api.Errors;
using DatingApp.Api.Interfaces;
using Mapster;

namespace DatingApp.Api.Services.UsersService
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userResponses = users.Adapt<IEnumerable<UserResponse>>();

            return userResponses;
        } 

        public async Task<Result<UserResponse>> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            var userResponse = user.Adapt<UserResponse>();

            return Result.Success(userResponse);
        }

        public async Task<Result<UserResponse>> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            var userResponse = user.Adapt<UserResponse>();

            return Result.Success(userResponse);
        }

        public async Task<Result> UpdateUserByUsernameAsync(string username, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            request.Adapt(user);

            _userRepository.Update(user);
            var updated = await _userRepository.SaveAllAsync();
            if(updated == 0)
                return Result.Failure<UserResponse>(UserErrors.FailedToSaveChanges);

            return Result.Success();
        }
    }
}
