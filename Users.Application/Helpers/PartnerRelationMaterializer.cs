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
}
