using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Entities;
using DatingApp.Api.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Services.AdminsService
{
    public class AdminsService(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager) : IAdminsService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        public async Task<IEnumerable<UserWithRolesResponse>> GetUsersWithRolesAsync()
        {
            var users = await _userManager.Users
                    .OrderBy(x => x.UserName)
                    .Select(u => new UserWithRolesResponse(
                        u.Id,    
                        u.UserName!,
                        u.UserRoles.Select(r => r.Role.Name)
                    )).ToListAsync();
            return users;
        }

        public async Task<Result<UserWithRolesResponse>> EditRolesAsync(string username, RoleRequest request)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.UserName == username);

            if (user is null)
                return Result.Failure<UserWithRolesResponse>(UserErrors.UserNotFound);

            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var isValidRoles = request.Roles.All(x => allRoles.Contains(x));
            if(!isValidRoles)
                return Result.Failure<UserWithRolesResponse>(RolesErrors.RoleNotFound);

            var userRoles = await _userManager.GetRolesAsync(user);

            var newRoles = request.Roles.Except(userRoles);
            var removedRoles = userRoles.Except(request.Roles);

            var result = await _userManager.AddToRolesAsync(user, newRoles);
            if (result.Errors.Any())
            {
                var error = result.Errors.First();
                return Result.Failure<UserWithRolesResponse>(new Error(error.Code,
                        error.Description, StatusCodes.Status400BadRequest));
            }

            result = await _userManager.RemoveFromRolesAsync(user, removedRoles);
            if (result.Errors.Any())
            {
                var error = result.Errors.First();
                return Result.Failure<UserWithRolesResponse>(new Error(error.Code,
                        error.Description, StatusCodes.Status400BadRequest));
            }

            var userWNewRoles = await _userManager.Users
                    .Where(u => u.UserName == username)
                    .Select(u => new UserWithRolesResponse(
                        u.Id,
                        u.UserName!,
                        u.UserRoles.Select(r => r.Role.Name)
                    )).SingleOrDefaultAsync();

            return Result.Success(userWNewRoles!);
        }
    }
}
