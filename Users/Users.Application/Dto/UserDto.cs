namespace Users.Application.Dto;

public class UserDto
{
    public Guid ExternalId { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public List<UserDto> Partners { get; set; }

    public List<UserDto> Referrals { get; set; }
}
