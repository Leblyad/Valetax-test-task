using Mapster;
using Users.Application.Dto;
using Users.Domain.Models;

namespace Users.Application.Mapping;

public sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserDto, User>()
            .Ignore(dest => dest.PartnerRelations)
            .AfterMapping(dest => dest.PartnerRelations = []);

        config.NewConfig<User, UserDto>()
            .Ignore(dest => dest.Partners)
            .Ignore(dest => dest.Referrals)
            .AfterMapping(dest =>
            {
                dest.Partners = [];
                dest.Referrals = [];
            });

        config.NewConfig<PartnerRelation, PartnerRelationDto>();
    }
}
