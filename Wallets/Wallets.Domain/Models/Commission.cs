using SharedModels.Enums;

namespace Wallets.Domain.Models;

public class Commission
{
    public Guid EventExternalId { get; set; }

    public Guid UserExternalId { get; set; }

    public SchemaType SchemaType { get; set; }

    public decimal Amount { get; set; }

    public int Level { get; set; }

    public DateTime? PaidAt { get; set; }

    public Wallet Wallet { get; set; }
}
