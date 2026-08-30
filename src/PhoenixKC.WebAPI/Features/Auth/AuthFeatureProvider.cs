using System.Text;
using PhoenixKC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PhoenixKC.Data.Features.Auth.Users;
using PhoenixKC.Data.Features.Auth.Roles;
using PhoenixKC.WebAPI.Features.Auth.Options;
using PhoenixKC.WebAPI.Features.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PhoenixKC.WebAPI.Features.Auth.Handlers.LoginUser;
using PhoenixKC.WebAPI.Features.Auth.Handlers.DeleteUser;
using PhoenixKC.WebAPI.Features.Auth.Handlers.RegisterUser;
using PhoenixKC.WebAPI.Features.Auth.Handlers.GenerateTokens;
using PhoenixKC.WebAPI.Features.Auth.Handlers.RefreshAccessToken;
using PhoenixKC.WebAPI.Features.Auth.Handlers.RemoveExpiredRefreshTokens;

namespace PhoenixKC.WebAPI.Features.Auth;

public sealed class AuthFeatureProvider : FeatureProvider
{
    #region Interfaces
    public override void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddOptionsWithValidateOnStart<JwtOptions>().BindConfiguration(JwtOptions.SectionName);
        builder.Services.AddOptionsWithValidateOnStart<RefreshTokenOptions>().BindConfiguration(RefreshTokenOptions.SectionName);
        IConfigurationSection jwt = builder.Configuration.GetRequiredSection(JwtOptions.SectionName);
        builder.Services.AddIdentityCore<UserEntity>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = AuthConstants.PasswordMinLength;
            options.Password.RequireNonAlphanumeric = false;
        }).AddRoles<RoleEntity>().AddSignInManager().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            string jwtKey = jwt[nameof(JwtOptions.Key)]!;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwt[nameof(JwtOptions.Issuer)],
                ValidateAudience = true,
                ValidAudience = jwt[nameof(JwtOptions.Audience)],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<IAccessTokenGenerator, AccessTokenGenerator>();
        builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }
    public override void UseMiddleware(WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        app.AddRegisterUserEndpoint();
        app.AddLoginUserEndpoint();
        app.AddRefreshAccessTokenEndpoint();
        app.AddDeleteUserEndpoint();
        if(app.Environment.IsEnvironment(ProfileNames.Test))
        {
            app.AddRemoveExpiredRefreshTokensEndpoint();
            app.AddGenerateTokensEndpoint();
        }
    }
    #endregion
}