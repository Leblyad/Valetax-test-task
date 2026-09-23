using Users.Application.Interfaces.Repositories;
using Users.Persistence.Context;

namespace Users.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;

    public RepositoryManager(
        RepositoryContext repositoryContext,
        IUserRepository userRepository,
        IPartnerRelationRepository partnerRelationRepository,
        IOutboxRepository outboxRepository)
    {
        _repositoryContext = repositoryContext;
        User = userRepository;
        PartnerRelation = partnerRelationRepository;
        Outbox = outboxRepository;
    }

    public IUserRepository User { get; }

    public IPartnerRelationRepository PartnerRelation { get; }

    public IOutboxRepository Outbox { get; }

    public Task SaveAsync(CancellationToken cancellationToken = default) =>
        _repositoryContext.SaveChangesAsync(cancellationToken);
}
