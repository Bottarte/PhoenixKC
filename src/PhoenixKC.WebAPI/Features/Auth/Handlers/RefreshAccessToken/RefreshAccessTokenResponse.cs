using PhoenixKC.WebAPI.Features.Auth.Dtos.Users;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.RefreshAccessToken;

public sealed record class RefreshAccessTokenResponse(UserDto User, string AccessToken);