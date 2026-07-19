using Mediator;
using FluentResults;
using PhoenixKC.Infrastructure;

namespace PhoenixKC.WebAPI.Features.Example.Handlers;

public sealed class CreateExampleHandler(PhoenixDbContext thisDbContext) : IRequestHandler<CreateExampleCommand, Result>
{
    #region Interfaces
    public async ValueTask<Result> Handle(CreateExampleCommand command, CancellationToken cancellationToken)
    {
        command.Example.Id = Guid.Empty;
        await thisDbContext.Examples.AddAsync(command.Example.ToEntity(), cancellationToken);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
    #endregion
}