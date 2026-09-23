using SharedModels.Enums;

namespace Wallets.Domain.Models;

public class Wallet
{
    public Guid UserExternalId { get; set; }

    public decimal Balance { get; set; }

    public SchemaType SchemaType { get; set; }

    public List<Commission> Commissions { get; set; }
}
