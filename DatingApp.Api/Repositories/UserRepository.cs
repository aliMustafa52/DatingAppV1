using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
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

        public IQueryable<ApplicationUser> GetAll()
        {
            var users = _context.Users
                    .Include(u => u.Photos)
                    .AsNoTracking();

            return users;
        }

        public async Task<ApplicationUser?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());

            return user;
        }

        public async Task<int> SaveAllAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(ApplicationUser user)
        {
            _context.Users.Update(user);
        }
    }
}
