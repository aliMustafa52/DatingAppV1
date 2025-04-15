using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Entities;

namespace DatingApp.Api.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<Result<ApplicationUser>> GetAsync(int id);
        Task<Result<ApplicationUser>> GetByUsernameAsync(string username);
        void Update(ApplicationUser user);

        Task<Result> SaveAllAsync();

        Task<Result<UserResponse>> GetResponseByUsernameAsync(string username);
    }
}
