using Bogus;
using FluentAssertions;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using PhoenixKC.WebAPI.Features.Example;
using PhoenixKC.WebAPI.Features.Example.Dtos;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.IntegrationTests.Features.Example;

public sealed class GetExampleByIdHandlerTests(PhoenixFixture thisFixture)
{
    [Fact]
    public async ValueTask Handler_ReturnsExample_WhenSucceded()
    {
        //Arrange
        List<ExampleEntity> entities = new ExampleEntityFaker().GenerateBetween(2, 10);
        await thisFixture.ResetDatabaseAsync();
        await thisFixture.ExecuteAsync(async db =>
        {
            await db.Examples.AddRangeAsync(entities, TestContext.Current.CancellationToken);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);
        });
        ExampleDto dto = Random.Shared.GetItems(CollectionsMarshal.AsSpan(entities), 1)[0].ToDto();

        //Act
        ExampleDto? result = await thisFixture.HttpClient.GetFromJsonAsync<ExampleDto>($"/example/{dto.Id}", TestContext.Current.CancellationToken);

        //Assert
        result.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async ValueTask Handler_ReturnsError_WhenIdIsInvalid()
    {
        //Arrange
        List<ExampleEntity> entities = new ExampleEntityFaker().GenerateBetween(2, 10);
        await thisFixture.ResetDatabaseAsync();
        await thisFixture.ExecuteAsync(async db =>
        {
            await db.Examples.AddRangeAsync(entities, TestContext.Current.CancellationToken);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);
        });

        //Act
        using HttpResponseMessage response = await thisFixture.HttpClient.GetAsync($"/example/{Guid.CreateVersion7()}", TestContext.Current.CancellationToken);

        //Assert
        response.IsSuccessStatusCode.Should().BeFalse();
    }
}