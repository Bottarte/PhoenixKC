using Microsoft.AspNetCore.Identity;
using PhoenixKC.Data.Features.Auth.RefreshTokens;

namespace PhoenixKC.Data.Features.Auth.Users;

public sealed class UserEntity : IdentityUser<Guid>
{
    public List<RefreshTokenEntity> RefreshTokens { get; set; } = [];
}