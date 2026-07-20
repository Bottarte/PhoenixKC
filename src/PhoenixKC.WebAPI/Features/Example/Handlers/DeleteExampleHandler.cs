using Mediator;
using FluentResults;
using PhoenixKC.Infrastructure;
using PhoenixKC.WebAPI.Resources;
using PhoenixKC.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.Infrastructure.Features.Example;

namespace PhoenixKC.WebAPI.Features.Example.Handlers;

public sealed class DeleteExampleHandler(
    PhoenixDbContext thisDbContext,
    ILogger<DeleteExampleHandler> thisLogger
) : IRequestHandler<DeleteExampleCommand, Result>
{
    #region Interfaces
    public async ValueTask<Result> Handle(DeleteExampleCommand command, CancellationToken cancellationToken)
    {
        ExampleEntity? entity = await thisDbContext.Examples.AsNoTracking().FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);
        if(entity is null)
        {
            thisLogger.LogFailAndThrow(ErrorMessages.RecordNotFound, nameof(ExampleEntity.Id), command.Id);
        }
        thisDbContext.Examples.Remove(entity);

        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
    #endregion
}