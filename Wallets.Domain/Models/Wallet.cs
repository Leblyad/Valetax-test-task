namespace Wallets.Domain.Models
{
    public class Wallet
    {
        public Guid ExternalId { get; set; }

        public decimal Balance { get; set; }

        public List<Commission> Commissions { get; set; }
    }
}
