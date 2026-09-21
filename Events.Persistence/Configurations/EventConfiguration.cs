using Events.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Persistence.Configurations;

public sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("events");

        builder.HasKey(x => x.ExternalId);

        builder.Property(x => x.ExternalId)
            .ValueGeneratedNever();

        builder.Property(x => x.UserExternalId)
            .IsRequired();

        builder.Property(x => x.Profit)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.UserExternalId);
    }
}
