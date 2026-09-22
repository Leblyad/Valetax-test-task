namespace Wallets.Application.Interfaces.Clients;

public interface IUsersApiClient
{
    Task<IReadOnlyList<PartnerRelationInfo>> GetPartnerRelationsAsync(
        Guid userExternalId,
        CancellationToken cancellationToken = default);
}

public sealed class PartnerRelationInfo
{
    public Guid UserExternalId { get; set; }

    public Guid PartnerExternalId { get; set; }

    public int Level { get; set; }
}
