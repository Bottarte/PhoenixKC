using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using PhoenixKC.WebAPI.Shared.Extensions;
using PhoenixKC.WebAPI.Features.Auth.Handlers.GenerateTokens;

namespace PhoenixKC.WebAPI.Features.Auth.Handlers.RefreshAccessToken;

public static class RefreshAccessTokenEndpoint
{
    public const string Url = "/api/auth/refresh-token";

    public static async Task<IResult> RefreshAccessToken(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<RefreshAccessTokenResponse> result = await mediator.Send(new RefreshAccessTokenCommand(), cancellationToken);
        return result.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddRefreshAccessTokenProductionProblems()
        {
            thisBuilder.ProducesProblem(StatusCodes.Status400BadRequest);
            thisBuilder.ProducesProblem(StatusCodes.Status404NotFound);
            return thisBuilder.AddGenerateTokensProductionProblems();
        }
    }
    extension(IEndpointRouteBuilder thisBuilder)
    {
        public void AddRefreshAccessTokenEndpoint()
        {
            thisBuilder.MapPut(Url, RefreshAccessToken)
                .WithName(nameof(RefreshAccessToken))
                .Produces<RefreshAccessTokenResponse>(StatusCodes.Status200OK)
                .AddRefreshAccessTokenProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendRefreshAccessTokenAsync(CancellationToken cancellationToken)
        {
            return await thisHttpClient.PutAsync(Url, null, cancellationToken);
        }
    }
}