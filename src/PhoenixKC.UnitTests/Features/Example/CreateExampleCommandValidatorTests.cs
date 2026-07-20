using FluentValidation.TestHelper;
using PhoenixKC.WebAPI.Features.Example.Dtos;
using PhoenixKC.Infrastructure.Features.Example;
using PhoenixKC.WebAPI.Features.Example.Handlers;
using PhoenixKC.WebAPI.Features.Example.Validators;

namespace PhoenixKC.UnitTests.Features.Example;

public sealed class CreateExampleCommandValidatorTests
{
    private CreateExampleCommandValidator Validator { get; } = new();

    [Fact]
    public void Validator_ShouldNotHaveAnyValidationErrors_WhenDtoIsValid()
    {
        //Arrange
        ExampleDto dto = new()
        {
            Title = new string('A', ExampleConstants.TitleMaxLength)
        };
        CreateExampleCommand command = new(dto);

        //Act
        TestValidationResult<CreateExampleCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrors_WhenDtoIsInvalid()
    {
        //Arrange
        ExampleDto dto = new()
        {
            Title = new string('A', ExampleConstants.TitleMaxLength + 1)
        };
        CreateExampleCommand command = new(dto);

        //Act
        TestValidationResult<CreateExampleCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrors();
    }
}