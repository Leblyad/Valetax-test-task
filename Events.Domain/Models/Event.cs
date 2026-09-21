namespace Events.Domain.Models
{
    public class Event
    {
        public Guid ExternalId { get; set; }

        public Guid UserExternalId { get; set; }

        public decimal Profit { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
