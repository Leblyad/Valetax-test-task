namespace Users.Application.Interfaces.Repositories;

public interface IRepositoryManager
{
    IUserRepository User { get; }

    IPartnerRepository Partner { get; }

    Task SaveAsync(CancellationToken cancellationToken = default);
}
