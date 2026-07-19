using Microsoft.AspNetCore.Mvc;
using PhoenixKC.WebAPI.Adapters;
using System.Diagnostics.CodeAnalysis;

namespace PhoenixKC.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class ActionResultExtensions
{
    public static IResult ToMvcResult(this IActionResult actionResult)
    {
        return new ActionResultAdapter(actionResult);
    }
}