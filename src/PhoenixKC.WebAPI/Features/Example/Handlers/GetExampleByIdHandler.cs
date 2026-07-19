using Mediator;
using FluentResults;
using PhoenixKC.Infrastructure;
using PhoenixKC.WebAPI.Resources;
using PhoenixKC.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.WebAPI.Features.Example.Dtos;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.WebAPI.Features.Example.Handlers;

public sealed class GetExampleByIdHandler(
    PhoenixDbContext thisDbContext,
    ILogger<GetExampleByIdHandler> thisLogger
) : IRequestHandler<GetExampleByIdQuery, Result<ExampleDto>>
{
    #region Interfaces
    public async ValueTask<Result<ExampleDto>> Handle(GetExampleByIdQuery query, CancellationToken cancellationToken)
    {
        ExampleEntity? entity = await thisDbContext.Examples.AsNoTracking().FirstOrDefaultAsync(e => e.Id == query.Id, cancellationToken);
        if(entity is null)
        {
            thisLogger.LogFailAndThrow(ErrorMessages.RecordNotFound, nameof(ExampleEntity.Id), query.Id);
        }
        return Result.Ok(entity.ToDto());
    }
    #endregion
}