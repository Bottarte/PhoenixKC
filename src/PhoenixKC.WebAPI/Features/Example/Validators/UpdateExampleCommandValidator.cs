using FluentValidation;
using PhoenixKC.WebAPI.Features.Example.Handlers;

namespace PhoenixKC.WebAPI.Features.Example.Validators;

public sealed class UpdateExampleCommandValidator : AbstractValidator<UpdateExampleCommand>
{
    public UpdateExampleCommandValidator()
    {
        base.RuleFor(c => c.Example).SetValidator(new ExampleDtoValidator());
    }
}