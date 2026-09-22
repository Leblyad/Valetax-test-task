using Users.Application.Dto;
using Users.Application.Interfaces.Services;

namespace Users.Application.Services;

public class PartnerRelationService : IPartnerRelationService
{
    public Task<Guid> SetPartnerRelationAsync(CreatePartnerRelationDto partnerRelation, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<PartnerRelationDto>> GetPartnerRelationsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<PartnerRelationDto>> GetReferralsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
