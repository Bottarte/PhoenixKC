using PhoenixKC.Data.Features.Auth.Users;

namespace PhoenixKC.WebAPI.Features.Auth.Services;

public interface IAccessTokenGenerator
{
    public abstract ValueTask<string> GenerateTokenAsync(UserEntity user, CancellationToken cancellationToken);
}