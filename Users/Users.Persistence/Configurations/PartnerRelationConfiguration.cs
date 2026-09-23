using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Models;

namespace Users.Persistence.Configurations;

public sealed class PartnerRelationConfiguration : IEntityTypeConfiguration<PartnerRelation>
{
    public void Configure(EntityTypeBuilder<PartnerRelation> builder)
    {
        builder.ToTable("partner_relations");

        builder.HasKey(x => new { x.UserExternalId, x.Level });

        builder.Property(x => x.Level)
            .IsRequired();

        builder.HasIndex(x => new { x.UserExternalId, x.PartnerExternalId })
            .IsUnique();

        builder.HasOne(x => x.PartnerUser)
            .WithMany()
            .HasForeignKey(x => x.PartnerExternalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
