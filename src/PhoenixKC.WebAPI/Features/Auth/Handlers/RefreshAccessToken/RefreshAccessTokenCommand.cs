using ErrorOr;
using Mediator;
using PhoenixKC.WebAPI.Shared.Behaviors.DbTransaction;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.RefreshAccessToken;

public sealed record class RefreshAccessTokenCommand : IDbTransactionBehaviorMessage, ICommand<ErrorOr<RefreshAccessTokenResponse>>;