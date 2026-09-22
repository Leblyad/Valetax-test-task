using SharedModels.Enums;

namespace Wallets.Application.Dto;

public class CommissionDto
{
    public Guid EventExternalId { get; set; }

    public Guid UserExternalId { get; set; }

    public SchemaType SchemaType { get; set; }

    public decimal Amount { get; set; }

    public int Level { get; set; }

    public DateTime? PaidAt { get; set; }
}
