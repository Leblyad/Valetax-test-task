using SharedModels.Enums;

namespace Users.Application.Dto;

public class CreateUserDto
{
    public Guid ExternalId { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public SchemaType SchemaType { get; set; }
}
