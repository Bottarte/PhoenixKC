using Bogus;
using FluentAssertions;
using System.Net.Http.Json;
using PhoenixKC.WebAPI.Features.Example;
using PhoenixKC.WebAPI.Features.Example.Dtos;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.IntegrationTests.Features.Example;

public sealed class GetAllExamplesHandlerTests(PhoenixFixture thisFixture)
{
    [Fact]
    public async ValueTask Handler_ReturnsExamples_WhenSucceded()
    {
        //Arrange
        List<ExampleEntity> entities = new ExampleEntityFaker().GenerateBetween(2, 10);
        await thisFixture.ResetDatabaseAsync();
        await thisFixture.ExecuteAsync(async db =>
        {
            await db.Examples.AddRangeAsync(entities, TestContext.Current.CancellationToken);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);
        });
        ExampleDto[] dtos = entities.ToDto().ToArray();

        //Act
        ExampleDto[]? result = await thisFixture.HttpClient.GetFromJsonAsync<ExampleDto[]>("/example", TestContext.Current.CancellationToken);

        //Assert
        result.Should().BeEquivalentTo(dtos);
    }
}