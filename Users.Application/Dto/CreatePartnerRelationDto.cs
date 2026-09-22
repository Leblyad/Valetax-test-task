namespace Users.Application.Dto;

public class CreatePartnerRelationDto
{
    public Guid UserExternalId { get; set; }

    public Guid PartnerExternalId { get; set; }
}
