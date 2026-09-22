using Events.Application.Dto;
using Events.Domain.Models;
using Mapster;

namespace Events.Application.Mapping;

public sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateEventDto, Event>()
            .Map(dest => dest.CreatedAt, src => src.CreatedAt == default ? DateTime.UtcNow : src.CreatedAt);

        config.NewConfig<Event, EventDto>();
    }
}
