using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.IntegrationTests.Features.Example;

public sealed class DeleteExampleHandlerTests(PhoenixFixture thisFixture)
{
    [Fact]
    public async ValueTask Handler_DeletesExample_WhenSucceded()
    {
        //Arrange
        List<ExampleEntity> entities = new ExampleEntityFaker().GenerateBetween(2, 10);
        await thisFixture.ResetDatabaseAsync();
        await thisFixture.ExecuteAsync(async db =>
        {
            await db.Examples.AddRangeAsync(entities, TestContext.Current.CancellationToken);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);
        });
        Guid id = Random.Shared.GetItems(CollectionsMarshal.AsSpan(entities), 1)[0].Id;

        //Act
        using HttpResponseMessage response = await thisFixture.HttpClient.DeleteAsync($"/example/{id}", TestContext.Current.CancellationToken);

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        await thisFixture.ExecuteAsync(async db =>
        {
            ExampleEntity? entity = await db.Examples.FirstOrDefaultAsync(e => e.Id == id, TestContext.Current.CancellationToken);
            entity.Should().BeNull();
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

        //Act
        using HttpResponseMessage response = await thisFixture.HttpClient.DeleteAsync($"/example/{Guid.CreateVersion7()}", TestContext.Current.CancellationToken);

        //Assert
        response.IsSuccessStatusCode.Should().BeFalse();
    }
}