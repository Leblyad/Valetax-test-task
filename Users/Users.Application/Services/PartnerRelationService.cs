using MapsterMapper;
using Users.Application.Dto;
using Users.Application.Exceptions;
using Users.Application.Helpers;
using Users.Application.Interfaces.Repositories;
using Users.Application.Interfaces.Services;
using Users.Domain.Models;

namespace Users.Application.Services;

public class PartnerRelationService(
    IRepositoryManager repositoryManager,
    IMapper mapper) : IPartnerRelationService
{
    public async Task<Guid> SetPartnerRelationAsync(
        CreatePartnerRelationDto partnerRelationDto,
        CancellationToken cancellationToken = default)
    {
        var user = await GetRequiredUserAsync(partnerRelationDto.UserExternalId, cancellationToken);
        var partner = await GetRequiredUserAsync(partnerRelationDto.PartnerExternalId, cancellationToken);

        var existingRelations = await repositoryManager.PartnerRelation.GetByUserExternalIdAsync(
            user.ExternalId,
            trackChanges: false,
            cancellationToken);

        if (existingRelations.Count > 0)
        {
            throw new PartnerRelationAlreadyExistsException(user.ExternalId);
        }

        var partnerAncestors = await repositoryManager.PartnerRelation.GetByUserExternalIdAsync(
            partner.ExternalId,
            trackChanges: false,
            cancellationToken);

        if (partnerAncestors.Any(r => r.PartnerExternalId == user.ExternalId))
        {
            throw new CyclicPartnerRelationException(user.ExternalId, partner.ExternalId);
        }

        var relations = PartnerRelationMaterializer.Materialize(
            user.ExternalId,
            partner.ExternalId,
            partnerAncestors).ToList();

        var descendantLinks = await repositoryManager.PartnerRelation.GetReferralsByPartnerExternalIdAsync(
            user.ExternalId,
            trackChanges: false,
            cancellationToken);

        if (descendantLinks.Count > 0)
        {
            relations.AddRange(PartnerRelationMaterializer.MaterializeForDescendants(
                descendantLinks,
                partner.ExternalId,
                partnerAncestors));
        }

        repositoryManager.PartnerRelation.CreateRange(relations);
        await repositoryManager.SaveAsync(cancellationToken);

        return user.ExternalId;
    }

    public async Task<List<PartnerRelationDto>> GetPartnerRelationsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var relations = await repositoryManager.PartnerRelation.GetByUserExternalIdAsync(
            userId,
            trackChanges: false,
            cancellationToken);

        return mapper.Map<List<PartnerRelationDto>>(relations);
    }

    public async Task<List<PartnerRelationDto>> GetReferralsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var relations = await repositoryManager.PartnerRelation.GetReferralsByPartnerExternalIdAsync(
            userId,
            trackChanges: false,
            cancellationToken);

        return mapper.Map<List<PartnerRelationDto>>(relations);
    }

    private async Task<User> GetRequiredUserAsync(Guid externalId, CancellationToken cancellationToken)
    {
        return await repositoryManager.User.GetByExternalIdAsync(
            externalId,
            trackChanges: false,
            cancellationToken)
            ?? throw new UserNotFoundException(externalId);
    }
}
