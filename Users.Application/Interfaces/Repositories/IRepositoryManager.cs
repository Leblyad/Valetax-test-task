namespace Users.Application.Interfaces.Repositories;

public interface IRepositoryManager
{
    IUserRepository User { get; }

    IPartnerRelationRepository PartnerRelation { get; }

    IOutboxRepository Outbox { get; }

    Task SaveAsync(CancellationToken cancellationToken = default);
}
