using FluentAssertions;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.WebAPI.Features.Example;
using PhoenixKC.WebAPI.Features.Example.Dtos;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.IntegrationTests.Features.Example;

public sealed class CreateExampleHandlerTests(PhoenixFixture thisFixture)
{
    [Fact]
    public async ValueTask Handler_CreatesExample_WhenSucceded()
    {
        //Arrange
        await thisFixture.ResetDatabaseAsync();
        ExampleDto dto = new ExampleDtoFaker().Generate();

        //Act
        using HttpResponseMessage response = await thisFixture.HttpClient.PostAsJsonAsync("/example", dto, TestContext.Current.CancellationToken);

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        await thisFixture.ExecuteAsync(async db =>
        {
            ExampleEntity? entity = await db.Examples.FirstOrDefaultAsync(TestContext.Current.CancellationToken);
            entity.Should().NotBeNull();
            entity.ToDto().Should().BeEquivalentTo(dto, cfg => cfg.Excluding(e => e.Id));
        });
    }

    [Fact]
    public async ValueTask Handler_CreatesExample_AndShouldResetId()
    {
        //Arrange
        await thisFixture.ResetDatabaseAsync();
        ExampleDto dto = new ExampleDtoFaker().Generate();
        dto.Id = Guid.CreateVersion7();

        //Act
        using HttpResponseMessage response = await thisFixture.HttpClient.PostAsJsonAsync("/example", dto, TestContext.Current.CancellationToken);

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        await thisFixture.ExecuteAsync(async db =>
        {
            ExampleEntity? entity = await db.Examples.FirstOrDefaultAsync(TestContext.Current.CancellationToken);
            entity.Should().NotBeNull();
            ExampleDto result = entity.ToDto();
            result.Should().BeEquivalentTo(dto, cfg => cfg.Excluding(e => e.Id));
            result.Id.Should().NotBe(dto.Id, "because database must generate id, not client");
        });
    }
}