using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Users;

namespace DatingApp.Api.Services.AdminsService
{
    public interface IAdminsService
    {
        Task<IEnumerable<UserWithRolesResponse>> GetUsersWithRolesAsync();

        Task<Result<UserWithRolesResponse>> EditRolesAsync(string username, RoleRequest request);
    }
}
