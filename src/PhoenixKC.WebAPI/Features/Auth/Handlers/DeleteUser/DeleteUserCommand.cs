using ErrorOr;
using Mediator;
using PhoenixKC.WebAPI.Shared.Behaviors.Authorized;
using PhoenixKC.WebAPI.Shared.Behaviors.DbTransaction;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.DeleteUser;

public sealed record class DeleteUserCommand : IDbTransactionBehaviorMessage, IAuthorizedBehaviorMessage, ICommand<ErrorOr<Unit>>;