using ErrorOr;
using Mediator;
using PhoenixKC.Data;
using Microsoft.Extensions.Options;
using PhoenixKC.WebAPI.Features.Auth.Options;
using PhoenixKC.WebAPI.Features.Auth.Services;
using PhoenixKC.Data.Features.Auth.RefreshTokens;
using PhoenixKC.WebAPI.Shared.Behaviors.DbTransaction;
using PhoenixKC.WebAPI.Features.Auth.Dtos.RefreshTokens;
using PhoenixKC.WebAPI.Features.Auth.Handlers.RemoveExpiredRefreshTokens;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.GenerateTokens;

public sealed class GenerateTokensHandler(
    AppDbContext thisDbContext,
    IMediator thisMediator,
    IOptionsSnapshot<RefreshTokenOptions> tokenOptions,
    IAccessTokenGenerator thisAccessTokenGenerator,
    IRefreshTokenGenerator thisRefreshTokenGenerator
) : ICommandHandler<GenerateTokensCommand, ErrorOr<GenerateTokensResponse>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<GenerateTokensResponse>> Handle(GenerateTokensCommand command, CancellationToken cancellationToken)
    {
        ErrorOr<Unit> result = await thisMediator.Send(new RemoveExpiredRefreshTokensCommand()
        {
            BeginDbTransaction = false
        }, cancellationToken);
        if(result.IsError)
        {
            return result.Errors;
        }

        string refreshToken = await thisRefreshTokenGenerator.GenerateTokenAsync(RefreshTokenEntityConstants.RefreshTokenMaxLength, cancellationToken);
        RefreshTokenEntity entity = new()
        {
            Value = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(tokenOptions.Value.ExpireDays),
            UserId = command.User.Id,
        };
        await thisDbContext.RefreshTokens.AddAsync(entity, cancellationToken);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        string accessToken = await thisAccessTokenGenerator.GenerateTokenAsync(command.User, cancellationToken);
        return new GenerateTokensResponse(accessToken, entity.ToDto());
    }
    #endregion
}