using Bogus;
using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.IntegrationTests.Seeders;
using PhoenixKC.Data.Features.Auth.Users;
using PhoenixKC.WebAPI.Features.Auth.Handlers.RegisterUser;

namespace PhoenixKC.IntegrationTests.Features.Auth.Handlers.RegisterUser;

public sealed class RegisterUserHandlerTests(AppFixture thisApp)
{
    [Fact]
    public async ValueTask Handler_ShouldCreateUser()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();
        RegisterUserCommand command = new Faker<RegisterUserCommand>().ValidInstance().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendRegisterUserAsync(command, TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be204NoContent();
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            UserEntity? user = await db.Users.SingleOrDefaultAsync(TestContext.Current.CancellationToken);
            user.Should().NotBeNull();
            user.Email.Should().Be(command.Email);
        });
    }

    [Fact]
    public async ValueTask Handler_ShouldFail_WhenUserExists()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();
        RegisterUserCommand command = await thisApp.AddUsersAndPickRandomAsync();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendRegisterUserAsync(command, TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be409Conflict();
    }

    [Fact]
    public async ValueTask Handler_ShouldFail_WhenEmailIsInvalid()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();
        RegisterUserCommand command = new Faker<RegisterUserCommand>().ValidInstance().WithInvalidEmail().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendRegisterUserAsync(command, TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be422UnprocessableEntity();
    }

    [Fact]
    public async ValueTask Handler_ShouldFail_WhenPasswordTooShord()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();
        RegisterUserCommand command = new Faker<RegisterUserCommand>().ValidInstance().WithTooShortPassword().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendRegisterUserAsync(command, TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be422UnprocessableEntity();
    }

    [Fact]
    public async ValueTask Handler_ShouldFail_WhenPasswordDoNotContainUppercaseLetters()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();
        RegisterUserCommand command = new Faker<RegisterUserCommand>().ValidInstance().WithPasswordWithoutUppercaseLetters().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendRegisterUserAsync(command, TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be422UnprocessableEntity();
    }

    [Fact]
    public async ValueTask Handler_ShouldFail_WhenPasswordDoNotContainLowercaseLetters()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();
        RegisterUserCommand command = new Faker<RegisterUserCommand>().ValidInstance().WithPasswordWithoutLowercaseLetters().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendRegisterUserAsync(command, TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be422UnprocessableEntity();
    }

    [Fact]
    public async ValueTask Handler_ShouldFail_WhenPasswordDoNotContainDigits()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();
        RegisterUserCommand command = new Faker<RegisterUserCommand>().ValidInstance().WithPasswordWithoutDigits().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendRegisterUserAsync(command, TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be422UnprocessableEntity();
    }
}