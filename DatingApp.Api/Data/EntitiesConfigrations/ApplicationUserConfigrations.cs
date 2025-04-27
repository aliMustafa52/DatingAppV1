using DatingApp.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Api.Data.EntitiesConfigrations
{
    public class ApplicationUserConfigrations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            //Simple many-to-many without extra fields
            //➔ Just use UsingEntity<Dictionary<string, object>>
            //builder
            //    .HasMany(u => u.LikedUsers)
            //    .WithMany(u => u.LikedByUsers)
            //    .UsingEntity<Dictionary<string, object>>("UserLike",
            //        j => j.HasOne<ApplicationUser>()
            //              .WithMany()
            //              .HasForeignKey("LikedUserId")
            //              .OnDelete(DeleteBehavior.Restrict),
            //        j => j.HasOne<ApplicationUser>()
            //              .WithMany()
            //              .HasForeignKey("LikedByUserId")
            //              .OnDelete(DeleteBehavior.Cascade)
            //    );
        }
    }
}
