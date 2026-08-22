using ErrorOr;
using Mediator;
using PhoenixKC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.Data.Features.Auth.Users;
using PhoenixKC.Data.Features.Auth.RefreshTokens;
using PhoenixKC.WebAPI.Shared.Behaviors.Authorized;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.DeleteUser;

public sealed class DeleteUserHandler(
    AppDbContext thisDbContext,
    UserManager<UserEntity> thisUserManager
) : ICommandHandler<DeleteUserCommand, ErrorOr<Unit>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Unit>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        UserEntity user = command.User;
        
        RefreshTokenEntity[] tokens = await thisDbContext.RefreshTokens.Where(e => e.UserId == user.Id).ToArrayAsync(cancellationToken);
        if(tokens.Length > 0)
        {
            thisDbContext.RefreshTokens.RemoveRange(tokens);
        }

        await thisUserManager.DeleteAsync(command.User);
        return Unit.Value;
    }
    #endregion
}