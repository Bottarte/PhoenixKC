using Bogus;
using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.Data.Features.Auth.Users;
using PhoenixKC.IntegrationTests.Seeders;
using PhoenixKC.Data.Features.Auth.RefreshTokens;
using PhoenixKC.WebAPI.Features.Auth.Dtos.RefreshTokens;
using PhoenixKC.WebAPI.Features.Auth.Handlers.DeleteUser;

namespace PhoenixKC.IntegrationTests.Features.Auth.Handlers.DeleteUser;

public sealed class DeleteUserHandlerTests(AppFixture thisApp)
{
    [Fact]
    public async ValueTask Handler_ShouldDeleteUserCascading()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();
        (UserEntity user, _, string accessToken) = await thisApp.AddUsers2AndLoginRandomAsync();
        (
            int userCount,
            int refreshTokenCount,
            int userRefreshTokenCount
        ) = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<RefreshTokenEntity>().ValidInstance().SeedDatabaseForAllUsersAsync(db, TestContext.Current.CancellationToken);
            int userCount = await db.Users.CountAsync(TestContext.Current.CancellationToken);
            int refreshTokenCount = await db.RefreshTokens.CountAsync(TestContext.Current.CancellationToken);
            int userRefreshTokenCount = await db.RefreshTokens.Where(e => e.UserId == user.Id).CountAsync(TestContext.Current.CancellationToken);
            return (userCount, refreshTokenCount, userRefreshTokenCount);
        });

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendDeleteUserAsync(accessToken, TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be204NoContent();
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            bool userExists = await db.Users.AnyAsync(e => e.Id == user.Id, TestContext.Current.CancellationToken);
            userExists.Should().BeFalse();
            int newUserCount = await db.Users.CountAsync(TestContext.Current.CancellationToken);
            newUserCount.Should().Be(userCount - 1);

            bool userRefreshTokensExist = await db.RefreshTokens.AnyAsync(e => e.UserId == user.Id, TestContext.Current.CancellationToken);
            userRefreshTokensExist.Should().BeFalse();
            int newRefreshTokenCount = await db.RefreshTokens.CountAsync(TestContext.Current.CancellationToken);
            newRefreshTokenCount.Should().Be(refreshTokenCount - userRefreshTokenCount);
        });
    }

    [Fact]
    public async ValueTask Handler_ShouldFail_WhenUnauthorized()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendDeleteUserAsync("", TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be401Unauthorized();
    }
}