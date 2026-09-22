using Microsoft.EntityFrameworkCore;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Models;
using Users.Persistence.Context;

namespace Users.Persistence.Repositories;

public class PartnerRelationRepository : RepositoryBase<PartnerRelation>, IPartnerRelationRepository
{
    public PartnerRelationRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IReadOnlyList<PartnerRelation>> GetByUserExternalIdAsync(
        Guid userExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default) =>
        await FindByCondition(r => r.UserExternalId == userExternalId, trackChanges)
            .Include(r => r.PartnerUser)
            .OrderBy(r => r.Level)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<PartnerRelation>> GetReferralsByPartnerExternalIdAsync(
        Guid partnerExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default) =>
        await FindByCondition(r => r.PartnerExternalId == partnerExternalId, trackChanges)
            .Include(r => r.User)
            .OrderBy(r => r.Level)
            .ToListAsync(cancellationToken);

    public void CreateRange(IEnumerable<PartnerRelation> relations) =>
        RepositoryContext.Set<PartnerRelation>().AddRange(relations);
}
