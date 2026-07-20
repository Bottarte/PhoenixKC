using FluentValidation;
using PhoenixKC.WebAPI.Features.Example.Handlers;

namespace PhoenixKC.WebAPI.Features.Example.Validators;

public sealed class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
{
    public CreateExampleCommandValidator()
    {
        base.RuleFor(c => c.Example).SetValidator(new ExampleDtoValidator());
    }
}