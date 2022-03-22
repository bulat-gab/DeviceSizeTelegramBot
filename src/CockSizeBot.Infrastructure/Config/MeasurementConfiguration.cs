using CockSizeBot.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CockSizeBot.Infrastructure.Config;

public class MeasurementConfiguration : IEntityTypeConfiguration<Measurement>
{
    public void Configure(EntityTypeBuilder<Measurement> builder)
    {
        builder
            .HasKey(u => u.Id)
            .IsClustered(true)
            .HasName("Id");

        builder.Property(u => u.Id)
            .IsRequired(true)
            .ValueGeneratedOnAdd();

        builder.HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.User.Id);
    }
}
