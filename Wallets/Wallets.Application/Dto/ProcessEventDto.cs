namespace Wallets.Application.Dto;

public class ProcessEventDto
{
    public Guid EventExternalId { get; set; }

    public Guid UserExternalId { get; set; }

    public decimal Profit { get; set; }
}
