using PhoenixKC.WebAPI.Features;

namespace PhoenixKC.WebAPI.Shared.Behaviors.Authorized;

public sealed class AuthorizedBehaviorFeatureProvider : FeatureProvider
{
    #region Base
    public override void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
    }
    #endregion
}