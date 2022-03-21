using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CockSizeBot.Infrastructure.Config;

public class UserConfiguration : IEntityTypeConfiguration<Domain.User>
{
    public void Configure(EntityTypeBuilder<Domain.User> builder)
    {
        builder.Property(u => u.Id)
            .IsRequired(true)
            .ValueGeneratedOnAdd();
    }
}
