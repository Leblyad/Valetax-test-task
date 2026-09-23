using FluentValidation;
using Wallets.Application.Dto;

namespace Wallets.Application.Validators;

public sealed class CreateWalletDtoValidator : AbstractValidator<CreateWalletDto>
{
    public CreateWalletDtoValidator()
    {
        RuleFor(x => x.UserExternalId)
            .NotEmpty();

        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.SchemaType)
            .IsInEnum();
    }
}
