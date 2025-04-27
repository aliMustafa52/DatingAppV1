using DatingApp.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DatingApp.Api.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : DbContext(options)
    {
        public required DbSet<ApplicationUser> Users { get; set; }
        public required DbSet<Photo> Photos { get; set; }
        public required DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
