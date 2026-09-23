using FluentValidation;
using Users.Application.Dto;

namespace Users.Application.Validators;

public sealed class CreatePartnerRelationDtoValidator : AbstractValidator<CreatePartnerRelationDto>
{
    public CreatePartnerRelationDtoValidator()
    {
        RuleFor(x => x.UserExternalId)
            .NotEmpty();

        RuleFor(x => x.PartnerExternalId)
            .NotEmpty()
            .NotEqual(x => x.UserExternalId);
    }
}
