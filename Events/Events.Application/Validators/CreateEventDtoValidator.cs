using Events.Application.Dto;
using FluentValidation;

namespace Events.Application.Validators;

public sealed class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty();

        RuleFor(x => x.UserExternalId)
            .NotEmpty();

        RuleFor(x => x.CreatedAt)
            .NotEmpty();
    }
}
