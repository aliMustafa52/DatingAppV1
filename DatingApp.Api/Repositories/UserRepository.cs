using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using DatingApp.Api.Errors;
using DatingApp.Api.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users
                    .Include(u => u.Photos)
                    .ToListAsync();
        }

        public async Task<Result<ApplicationUser>> GetAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.Id == id);
            if (user is null)
                return Result.Failure<ApplicationUser>(UserErrors.UserNotFound);

            return Result.Success(user);
        }

        public async Task<Result<ApplicationUser>> GetByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.UserName == username);
            if (user is null)
                return Result.Failure<ApplicationUser>(UserErrors.UserNotFound);

            return Result.Success(user);
        }

        public async Task<Result<UserResponse>> GetResponseByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Where(u => u.UserName == username)
                .ProjectToType<UserResponse>()
                .SingleOrDefaultAsync();
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            return Result.Success(user);
        }

        public async Task<Result> SaveAllAsync()
        {
            if (await _context.SaveChangesAsync() > 0)
                return Result.Success();

            //TODO: change this error
            return Result.Failure(UserErrors.FailedToSaveChanges);
        }

        public void Update(ApplicationUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
