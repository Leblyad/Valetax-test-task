using Users.Domain.Models;

namespace Users.Application.Interfaces.Repositories;

public interface IPartnerRelationRepository : IRepositoryBase<PartnerRelation>
{
    Task<IReadOnlyList<PartnerRelation>> GetByUserExternalIdAsync(
        Guid userExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PartnerRelation>> GetReferralsByPartnerExternalIdAsync(
        Guid partnerExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    void CreateRange(IEnumerable<PartnerRelation> relations);
}
