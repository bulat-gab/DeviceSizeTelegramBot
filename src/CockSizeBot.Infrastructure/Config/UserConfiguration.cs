using CockSizeBot.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CockSizeBot.Infrastructure.Config;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(u => u.Id)
            .IsClustered(true)
            .HasName("Id");

        builder.Property(u => u.Id)
            .IsRequired(true)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Username)
            .IsRequired(false)
            .HasMaxLength(512);
    }
}
