using Wallets.Domain.Enums;

namespace Wallets.Domain.Models
{
    public class Commission
    {
        public Guid ExternalId { get; set; }

        public Guid EventExternalId { get; set; }

        public Guid WalletExternalId { get; set; }

        public SchemaType SchemaType { get; set; }

        public decimal Amount { get; set; }

        public int Level { get; set; }

        public DateTime? PaidAt { get; set; }

        public Wallet Wallet { get; set; }
    }
}
