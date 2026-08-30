using Bogus;
using AwesomeAssertions;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.IntegrationTests.Seeders;
using PhoenixKC.Data.Features.Auth.Users;
using PhoenixKC.Data.Features.Auth.RefreshTokens;
using PhoenixKC.WebAPI.Features.Auth.Dtos.RefreshTokens;
using PhoenixKC.WebAPI.Features.Auth.Handlers.GenerateTokens;

namespace PhoenixKC.IntegrationTests.Features.Auth.Handlers.GenerateTokens;

public sealed class GenerateTokensHandlerTests(AppFixture thisApp)
{
    [Fact]
    public async ValueTask Handler_ShouldCreateAndReturnRefreshToken()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();
        (UserEntity user, _) = await thisApp.AddUsers2AndPickRandomAsync();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGenerateTokensAsync(user, TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be200Ok();
        GenerateTokensResponse? response = await message.Content.ReadFromJsonAsync<GenerateTokensResponse>(TestContext.Current.CancellationToken);
        response.Should().NotBeNull();
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            RefreshTokenEntity? refreshToken = await db.RefreshTokens.SingleOrDefaultAsync(TestContext.Current.CancellationToken);
            refreshToken.Should().NotBeNull();
            refreshToken.UserId.Should().Be(user.Id);
            refreshToken.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
            refreshToken.ToDto().Should().BeEquivalentTo(response.RefreshToken);
        });
    }

    [Fact]
    public async ValueTask Handler_ShouldRemoveAllExpiredRefreshTokens()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync();
        (UserEntity user, _) = await thisApp.AddUsers2AndPickRandomAsync();
        int expectedTokenCount = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<RefreshTokenEntity>().ValidInstance().SeedDatabaseForAllUsersAsync(db, TestContext.Current.CancellationToken);
            int expectedTokenCount = await db.RefreshTokens.CountAsync(TestContext.Current.CancellationToken);
            await new Faker<RefreshTokenEntity>().ValidInstance().MakeExpired().SeedDatabaseForAllUsersAsync(db, TestContext.Current.CancellationToken);
            return expectedTokenCount;
        }) + 1;

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGenerateTokensAsync(user, TestContext.Current.CancellationToken);

        //Assert
        message.Should().Be200Ok();
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            bool expiredTokensExist = await db.RefreshTokens.AnyAsync(e => DateTime.UtcNow > e.ExpiresAt, TestContext.Current.CancellationToken);
            expiredTokensExist.Should().BeFalse();

            int actualTokenCount = await db.RefreshTokens.CountAsync(TestContext.Current.CancellationToken);
            actualTokenCount.Should().Be(expectedTokenCount);
        });
    }
}