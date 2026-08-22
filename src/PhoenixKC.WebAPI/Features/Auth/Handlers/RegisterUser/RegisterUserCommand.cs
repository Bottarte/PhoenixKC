using ErrorOr;
using Mediator;
using PhoenixKC.WebAPI.Shared.Behaviors.DbTransaction;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.RegisterUser;

public sealed record class RegisterUserCommand(string Email, string Password) : IDbTransactionBehaviorMessage, ICommand<ErrorOr<Unit>>;