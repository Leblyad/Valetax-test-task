using System.Text.Json;
using MapsterMapper;
using SharedModels.Outbox;
using Users.Application.Dto;
using Users.Application.Exceptions;
using Users.Application.Interfaces.Repositories;
using Users.Application.Interfaces.Services;
using Users.Domain.Models;

namespace Users.Application.Services;

public class UserService(
    IRepositoryManager repositoryManager,
    IMapper mapper) : IUserService
{
    public async Task<Guid> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
    {
        var existing = await repositoryManager.User.GetByExternalIdAsync(
            createUserDto.ExternalId,
            trackChanges: false,
            cancellationToken);

        if (existing is not null)
        {
            return existing.ExternalId;
        }

        var user = mapper.Map<User>(createUserDto);
        repositoryManager.User.Create(user);
        repositoryManager.Outbox.Create(CreateWalletOutboxMessage(createUserDto));
        await repositoryManager.SaveAsync(cancellationToken);

        return createUserDto.ExternalId;
    }

    public async Task<UserDto> GetUserAsync(Guid externalId, CancellationToken cancellationToken = default)
    {
        var user = await repositoryManager.User.GetByExternalIdAsync(
            externalId,
            trackChanges: false,
            cancellationToken)
            ?? throw new UserNotFoundException(externalId);

        var partners = await repositoryManager.PartnerRelation.GetByUserExternalIdAsync(
            externalId,
            trackChanges: false,
            cancellationToken);

        var referrals = await repositoryManager.PartnerRelation.GetReferralsByPartnerExternalIdAsync(
            externalId,
            trackChanges: false,
            cancellationToken);

        var dto = mapper.Map<UserDto>(user);
        dto.Partners = MapDistinctUsers(partners.Select(r => r.PartnerUser));
        dto.Referrals = MapDistinctUsers(referrals.Select(r => r.User));
        return dto;
    }

    private List<UserDto> MapDistinctUsers(IEnumerable<User?> users) =>
        users
            .Where(u => u is not null)
            .DistinctBy(u => u!.ExternalId)
            .Select(u => mapper.Map<UserDto>(u!))
            .ToList();

    private static OutboxMessage CreateWalletOutboxMessage(CreateUserDto createUserDto) =>
        new()
        {
            Id = Guid.NewGuid(),
            Type = OutboxMessageTypes.CREATE_WALLET,
            Payload = JsonSerializer.Serialize(new
            {
                createUserDto.ExternalId,
                createUserDto.SchemaType,
            }),
            CreatedAtUtc = DateTime.UtcNow,
        };
}
