using FluentValidation;
using PhoenixKC.WebAPI.Features.Example.Dtos;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.WebAPI.Features.Example.Validators;

public sealed class ExampleDtoValidator : AbstractValidator<ExampleDto>
{
    public ExampleDtoValidator()
    {
        base.RuleFor(e => e.Title).MaximumLength(ExampleConstants.TitleMaxLength);
    }
}