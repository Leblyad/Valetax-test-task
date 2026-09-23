using Users.Domain.Models;

namespace Users.Application.Helpers;

public static class PartnerRelationMaterializer
{
    public const int MAX_LEVEL = 10;

    public static IReadOnlyList<PartnerRelation> Materialize(
        Guid userExternalId,
        Guid partnerExternalId,
        IReadOnlyList<PartnerRelation> partnerAncestors)
    {
        var relations = new List<PartnerRelation>
        {
            new()
            {
                UserExternalId = userExternalId,
                PartnerExternalId = partnerExternalId,
                Level = 1,
            },
        };

        foreach (var ancestor in partnerAncestors)
        {
            var level = ancestor.Level + 1;
            if (level > MAX_LEVEL)
            {
                continue;
            }

            relations.Add(new PartnerRelation
            {
                UserExternalId = userExternalId,
                PartnerExternalId = ancestor.PartnerExternalId,
                Level = level,
            });
        }

        return relations;
    }

    public static IReadOnlyList<PartnerRelation> MaterializeForDescendants(
        IReadOnlyList<PartnerRelation> descendantLinksToUser,
        Guid partnerExternalId,
        IReadOnlyList<PartnerRelation> partnerAncestors)
    {
        var relations = new List<PartnerRelation>();

        foreach (var link in descendantLinksToUser)
        {
            var partnerLevel = link.Level + 1;
            if (partnerLevel <= MAX_LEVEL)
            {
                relations.Add(new PartnerRelation
                {
                    UserExternalId = link.UserExternalId,
                    PartnerExternalId = partnerExternalId,
                    Level = partnerLevel,
                });
            }

            foreach (var ancestor in partnerAncestors)
            {
                var level = link.Level + 1 + ancestor.Level;
                if (level > MAX_LEVEL)
                {
                    continue;
                }

                relations.Add(new PartnerRelation
                {
                    UserExternalId = link.UserExternalId,
                    PartnerExternalId = ancestor.PartnerExternalId,
                    Level = level,
                });
            }
        }

        return relations;
    }
}
