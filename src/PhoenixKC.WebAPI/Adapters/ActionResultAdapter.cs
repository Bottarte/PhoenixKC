using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace PhoenixKC.WebAPI.Adapters;

[ExcludeFromCodeCoverage]
public sealed class ActionResultAdapter(IActionResult thisActionResult) : IResult
{
    #region Interfaces
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        ActionContext action_context = new()
        {
            HttpContext = httpContext,
            RouteData = httpContext.GetRouteData(),
            ActionDescriptor = new ActionDescriptor()
        };
        await thisActionResult.ExecuteResultAsync(action_context);
    }
    #endregion
}