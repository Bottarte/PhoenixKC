using ErrorOr;
using Mediator;
using PhoenixKC.Data.Features.Auth.Users;
using PhoenixKC.WebAPI.Shared.Behaviors.DbTransaction;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.GenerateTokens;

public sealed record class GenerateTokensCommand(UserEntity User) : IDbTransactionBehaviorMessage, ICommand<ErrorOr<GenerateTokensResponse>>;