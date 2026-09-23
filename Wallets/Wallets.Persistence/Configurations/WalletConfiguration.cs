using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallets.Domain.Models;

namespace Wallets.Persistence.Configurations;

public sealed class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("wallets");

        builder.HasKey(x => x.UserExternalId);

        builder.Property(x => x.UserExternalId)
            .ValueGeneratedNever();

        builder.Property(x => x.Balance)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(x => x.SchemaType)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.HasMany(x => x.Commissions)
            .WithOne(x => x.Wallet)
            .HasForeignKey(x => x.UserExternalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
