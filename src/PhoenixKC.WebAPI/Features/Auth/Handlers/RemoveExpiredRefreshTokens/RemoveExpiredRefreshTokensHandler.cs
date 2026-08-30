using ErrorOr;
using Mediator;
using PhoenixKC.Data;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.Data.Features.Auth.RefreshTokens;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.RemoveExpiredRefreshTokens;

public sealed class RemoveExpiredRefreshTokensHandler(AppDbContext thisDbContext) : ICommandHandler<RemoveExpiredRefreshTokensCommand, ErrorOr<Unit>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Unit>> Handle(RemoveExpiredRefreshTokensCommand command, CancellationToken cancellationToken)
    {
        RefreshTokenEntity[] tokens = await thisDbContext.RefreshTokens.Where(e => DateTime.UtcNow > e.ExpiresAt).ToArrayAsync(cancellationToken);
        thisDbContext.RefreshTokens.RemoveRange(tokens);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
    #endregion
}