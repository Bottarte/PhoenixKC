using Bogus;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.IntegrationTests.Features.Example;

public sealed class ExampleEntityFaker : Faker<ExampleEntity>
{
    public ExampleEntityFaker()
    {
        base.StrictMode(true);
        base.RuleFor(e => e.Id, f => Guid.Empty);
        base.RuleFor(e => e.Title, f => f.Random.AlphaNumeric(ExampleConstants.TitleMaxLength));
    }
}