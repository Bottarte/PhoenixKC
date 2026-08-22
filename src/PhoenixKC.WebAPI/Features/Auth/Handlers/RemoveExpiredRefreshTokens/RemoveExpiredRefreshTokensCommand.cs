using ErrorOr;
using Mediator;
using PhoenixKC.WebAPI.Shared.Behaviors.DbTransaction;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.RemoveExpiredRefreshTokens;

public sealed record class RemoveExpiredRefreshTokensCommand : IDbTransactionBehaviorMessage, ICommand<ErrorOr<Unit>>;