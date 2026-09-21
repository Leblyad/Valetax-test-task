namespace Users.Domain.Models
{
    public class Partner
    {
        public Guid UserExternalId { get; set; }

        public Guid PartnerExternalId { get; set; }

        public int Level { get; set; }

        public User User { get; set; }

        public User PartnerUser { get; set; }
    }
}
