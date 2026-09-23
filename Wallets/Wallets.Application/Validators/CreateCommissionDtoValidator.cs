using FluentValidation;
using Wallets.Application.Dto;

namespace Wallets.Application.Validators;

public sealed class CreateCommissionDtoValidator : AbstractValidator<CreateCommissionDto>
{
    public CreateCommissionDtoValidator()
    {
        RuleFor(x => x.EventExternalId)
            .NotEmpty();

        RuleFor(x => x.UserExternalId)
            .NotEmpty();

        RuleFor(x => x.SchemaType)
            .IsInEnum();

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Level)
            .InclusiveBetween(1, 10);
    }
}
