using SharedModels.Enums;

namespace Wallets.Application.Dto;

public class WalletWithCommissionsDto
{
    public Guid UserExternalId { get; set; }

    public decimal Balance { get; set; }

    public SchemaType SchemaType { get; set; }

    public List<CommissionDto> Commissions { get; set; }
}
