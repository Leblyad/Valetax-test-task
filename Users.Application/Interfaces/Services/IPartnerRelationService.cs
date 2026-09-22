using Users.Application.Dto;

namespace Users.Application.Interfaces.Services;

public interface IPartnerRelationService
{
    Task<Guid> SetPartnerRelationAsync(CreatePartnerRelationDto partnerRelationDto, CancellationToken cancellationToken = default);

    Task<List<PartnerRelationDto>> GetPartnerRelationsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<List<PartnerRelationDto>> GetReferralsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
