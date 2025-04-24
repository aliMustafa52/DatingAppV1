using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Entities;

namespace DatingApp.Api.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> GetAll();
        Task<ApplicationUser?> GetByIdAsync(int id);
        Task<ApplicationUser?> GetByUsernameAsync(string username);
        void Update(ApplicationUser user);
        Task<int> SaveAllAsync();
    }
}
