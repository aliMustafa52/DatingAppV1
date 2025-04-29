using DatingApp.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Api.Data.EntitiesConfigrations
{
    public class MessageConfigrations : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasOne(m => m.Sender)
                .WithMany(s => s.SentMessages)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(m => m.Recipient)
                .WithMany(s => s.RecivedMessages)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
