using PhoenixKC.Data.Features.Auth.Users;

namespace PhoenixKC.WebAPI.Shared.Behaviors.Authorized;

public sealed class AuthorizedBehaviorMessageExtraProperties
{
    public UserEntity User { get; set; } = null!;
}