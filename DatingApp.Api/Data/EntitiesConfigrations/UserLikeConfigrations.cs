using DatingApp.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Api.Data.EntitiesConfigrations
{
    public class UserLikeConfigrations : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder
                .HasKey(ul => new { ul.SourceUserId, ul.TargetUserId });

            builder
                .HasOne(ul => ul.SourceUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(u => u.SourceUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ul => ul.TargetUser)
                .WithMany(u => u.LikedByUsers)
                .HasForeignKey(u => u.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
