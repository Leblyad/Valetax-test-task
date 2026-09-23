using SharedModels.Enums;

namespace Wallets.Application.Dto;

public class WalletDto
{
    public Guid UserExternalId { get; set; }

    public decimal Balance { get; set; }

    public SchemaType SchemaType { get; set; }
}
