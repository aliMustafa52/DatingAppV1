using DatingApp.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DatingApp.Api.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : IdentityDbContext<ApplicationUser,ApplicationRole,int, IdentityUserClaim<int>,
                ApplicationUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
                    IdentityUserToken<int>>(options)
    {
        public required DbSet<Photo> Photos { get; set; }
        public required DbSet<UserLike> Likes { get; set; }
        public required DbSet<Message> Messages { get; set; }
        public required DbSet<Group> Groups { get; set; }
        public required DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
