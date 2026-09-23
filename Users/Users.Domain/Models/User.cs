namespace Users.Domain.Models;

public class User
{
    public Guid ExternalId { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public List<PartnerRelation> PartnerRelations { get; set; }
}
