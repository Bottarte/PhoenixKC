using PhoenixKC.WebAPI.Features.Auth.Dtos.Users;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.LoginUser;

public sealed record class LoginUserResponse(UserDto User, string AccessToken);