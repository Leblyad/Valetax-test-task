using FluentValidation;
using Wallets.Application.Dto;

namespace Wallets.Application.Validators;

public sealed class ProcessEventDtoValidator : AbstractValidator<ProcessEventDto>
{
    public ProcessEventDtoValidator()
    {
        RuleFor(x => x.EventExternalId)
            .NotEmpty();

        RuleFor(x => x.UserExternalId)
            .NotEmpty();
    }
}
