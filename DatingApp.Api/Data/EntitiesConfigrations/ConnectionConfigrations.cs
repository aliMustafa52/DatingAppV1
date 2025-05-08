using DatingApp.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Api.Data.EntitiesConfigrations
{
    public class ConnectionConfigrations : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder
                .HasKey(x => x.ConnectinId);
        }
    }
}
