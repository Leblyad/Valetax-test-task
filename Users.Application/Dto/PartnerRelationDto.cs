namespace Users.Application.Dto;

public class PartnerRelationDto
{
    public Guid UserExternalId { get; set; }

    public Guid PartnerExternalId { get; set; }

    public int Level { get; set; }
}
