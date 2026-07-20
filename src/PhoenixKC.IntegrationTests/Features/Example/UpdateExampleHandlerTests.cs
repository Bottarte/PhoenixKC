using Bogus;
using FluentAssertions;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using PhoenixKC.WebAPI.Features.Example;
using PhoenixKC.WebAPI.Features.Example.Dtos;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.IntegrationTests.Features.Example;

public sealed class UpdateExampleHandlerTests(PhoenixFixture thisFixture)
{
    [Fact]
    public async ValueTask Handler_UpdatesExample_WhenSucceded()
    {
        //Arrange
        List<ExampleEntity> entities = new ExampleEntityFaker().GenerateBetween(2, 10);
        await thisFixture.ResetDatabaseAsync();
        await thisFixture.ExecuteAsync(async db =>
        {
            await db.Examples.AddRangeAsync(entities, TestContext.Current.CancellationToken);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);
        });
        ExampleDto dto = new ExampleDtoFaker().Generate();
        dto.Id = Random.Shared.GetItems(CollectionsMarshal.AsSpan(entities), 1)[0].Id;

        //Act
        using HttpResponseMessage response = await thisFixture.HttpClient.PutAsJsonAsync("/example", dto, TestContext.Current.CancellationToken);

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        await thisFixture.ExecuteAsync(async db =>
        {
            ExampleEntity? entity = await db.Examples.FirstOrDefaultAsync(e => e.Id == dto.Id, TestContext.Current.CancellationToken);
            entity.Should().NotBeNull();
            entity.ToDto().Should().BeEquivalentTo(dto);
        });
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
        ExampleDto dto = new ExampleDtoFaker().Generate();
        dto.Id = Guid.CreateVersion7();

        //Act
        using HttpResponseMessage response = await thisFixture.HttpClient.PutAsJsonAsync("/example", dto, TestContext.Current.CancellationToken);

        //Assert
        response.IsSuccessStatusCode.Should().BeFalse();
    }
}