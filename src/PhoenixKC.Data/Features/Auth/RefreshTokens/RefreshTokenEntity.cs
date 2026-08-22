using PhoenixKC.Data.Features.Auth.Users;
using PhoenixKC.Data.Shared.KeyedEntities;
using PhoenixKC.Data.Features.Auth.Users.ForeignKey;

namespace PhoenixKC.Data.Features.Auth.RefreshTokens;

public sealed class RefreshTokenEntity : IKeyedEntity, IUserEntityForeignKey
{
    public Guid Id { get; set; }
    public required string Value { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }

    public UserEntity? User { get; set; }
}