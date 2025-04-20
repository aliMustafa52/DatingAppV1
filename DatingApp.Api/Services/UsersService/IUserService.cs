using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Entities;

namespace DatingApp.Api.Services.UsersService
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<Result<UserResponse>> GetUserByIdAsync(int id);
        Task<Result<UserResponse>> GetUserByUsernameAsync(string username);
        Task<Result> UpdateUserByUsernameAsync(string username, UpdateUserRequest request);
    }
}
