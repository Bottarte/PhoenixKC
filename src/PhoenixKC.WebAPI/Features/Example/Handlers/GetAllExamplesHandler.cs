using Mediator;
using FluentResults;
using PhoenixKC.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.WebAPI.Features.Example.Dtos;

namespace PhoenixKC.WebAPI.Features.Example.Handlers;

public sealed class GetAllExamplesHandler(PhoenixDbContext thisDbContext) : IRequestHandler<GetAllExamplesQuery, Result<IEnumerable<ExampleDto>>>
{
    #region Interfaces
    public async ValueTask<Result<IEnumerable<ExampleDto>>> Handle(GetAllExamplesQuery query, CancellationToken cancellationToken)
    {
        List<ExampleDto> examples = await thisDbContext.Examples.AsNoTracking().ProjectToDto().ToListAsync(cancellationToken);
        return Result.Ok(examples.AsEnumerable());
    }
    #endregion
}