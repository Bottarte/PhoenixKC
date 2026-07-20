using Mediator;
using FluentResults;
using PhoenixKC.Infrastructure;
using PhoenixKC.WebAPI.Resources;
using PhoenixKC.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.WebAPI.Features.Example.Handlers;

public sealed class UpdateExampleHandler(
    PhoenixDbContext thisDbContext,
    ILogger<UpdateExampleHandler> thisLogger
) : IRequestHandler<UpdateExampleCommand, Result>
{
    #region Interfaces
    public async ValueTask<Result> Handle(UpdateExampleCommand command, CancellationToken cancellationToken)
    {
        ExampleEntity? entity = await thisDbContext.Examples.AsNoTracking().FirstOrDefaultAsync(e => e.Id == command.Example.Id, cancellationToken);
        if(entity is null)
        {
            thisLogger.LogFailAndThrow(ErrorMessages.RecordNotFound, nameof(ExampleEntity.Id), command.Example.Id);
        }
        command.Example.MapToEntity(entity);
        thisDbContext.Examples.Update(entity);

        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
    #endregion
}