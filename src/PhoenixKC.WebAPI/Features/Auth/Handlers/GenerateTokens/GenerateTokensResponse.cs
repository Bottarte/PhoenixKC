using PhoenixKC.WebAPI.Features.Auth.Dtos.RefreshTokens;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.GenerateTokens;

public sealed record class GenerateTokensResponse(string AccessToken, RefreshTokenDto RefreshToken);