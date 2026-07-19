using Bogus;
using PhoenixKC.WebAPI.Features.Example.Dtos;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.IntegrationTests.Features.Example;

public sealed class ExampleDtoFaker : Faker<ExampleDto>
{
    public ExampleDtoFaker()
    {
        base.StrictMode(true);
        base.RuleFor(e => e.Id, f => Guid.Empty);
        base.RuleFor(e => e.Title, f => f.Random.AlphaNumeric(ExampleConstants.TitleMaxLength));
    }
}