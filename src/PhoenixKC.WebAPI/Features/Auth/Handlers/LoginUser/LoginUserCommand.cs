using ErrorOr;
using Mediator;
using PhoenixKC.WebAPI.Shared.Behaviors.DbTransaction;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.LoginUser;

public sealed record class LoginUserCommand(string Email, string Password) : IDbTransactionBehaviorMessage, ICommand<ErrorOr<LoginUserResponse>>;