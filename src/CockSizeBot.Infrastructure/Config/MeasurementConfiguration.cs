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

        builder.Property(u => u.UserId)
            .IsRequired(true);

        builder.HasOne<Measurement>("UserId")
            .WithMany()
            .HasForeignKey(m => m.UserId);
    }
}
