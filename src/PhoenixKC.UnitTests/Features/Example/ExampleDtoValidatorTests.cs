using FluentValidation.TestHelper;
using PhoenixKC.WebAPI.Features.Example.Dtos;
using PhoenixKC.Infrastructure.Features.Example;
using PhoenixKC.WebAPI.Features.Example.Validators;

namespace PhoenixKC.UnitTests.Features.Example;

public sealed class ExampleDtoValidatorTests
{
    private ExampleDtoValidator Validator { get; } = new();

    [Fact]
    public void Validator_ShouldNotHaveAnyValidationErrors_WhenTitleIsValid()
    {
        //Arrange
        ExampleDto dto = new()
        {
            Title = new string('A', ExampleConstants.TitleMaxLength)
        };

        //Act
        TestValidationResult<ExampleDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorForTitle_WhenTitleIsInvalid()
    {
        //Arrange
        ExampleDto dto = new()
        {
            Title = new string('A', ExampleConstants.TitleMaxLength + 1)
        };

        //Act
        TestValidationResult<ExampleDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldHaveValidationErrorFor(e => e.Title);
    }
}