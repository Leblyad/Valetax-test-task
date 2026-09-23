using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallets.Domain.Models;

namespace Wallets.Persistence.Configurations;

public sealed class CommissionConfiguration : IEntityTypeConfiguration<Commission>
{
    public void Configure(EntityTypeBuilder<Commission> builder)
    {
        builder.ToTable("commissions");

        builder.HasKey(x => new { x.EventExternalId, x.UserExternalId });

        builder.Property(x => x.EventExternalId)
            .IsRequired();

        builder.Property(x => x.UserExternalId)
            .IsRequired();

        builder.Property(x => x.SchemaType)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(x => x.Level)
            .IsRequired();

        builder.Property(x => x.PaidAt)
            .IsRequired(false);

        builder.HasIndex(x => x.UserExternalId);

        builder.HasIndex(x => new { x.EventExternalId, x.Level })
            .IsUnique();
    }
}
