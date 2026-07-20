using Mediator;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using PhoenixKC.WebAPI.Extensions;
using System.Diagnostics.CodeAnalysis;
using FluentResults.Extensions.AspNetCore;
using PhoenixKC.WebAPI.Features.Example.Dtos;
using PhoenixKC.WebAPI.Features.Example.Handlers;

namespace PhoenixKC.WebAPI.Features.Example;

[ExcludeFromCodeCoverage]
public sealed class ExampleEndpoints : IEndpointsProvider
{
    #region Static
    public static async Task<IResult> GetAllExamples(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        Result<IEnumerable<ExampleDto>> result = await mediator.Send(new GetAllExamplesQuery(), cancellationToken);
        return result.ToActionResult().ToMvcResult();
    }
    public static async Task<IResult> GetExampleById(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        Result<ExampleDto> result = await mediator.Send(new GetExampleByIdQuery(id), cancellationToken);
        return result.ToActionResult().ToMvcResult();
    }
    public static async Task<IResult> CreateExample(
        [FromBody] ExampleDto dto,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        Result result = await mediator.Send(new CreateExampleCommand(dto), cancellationToken);
        return result.ToActionResult().ToMvcResult();
    }
    public static async Task<IResult> UpdateExample(
        [FromBody] ExampleDto dto,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        Result result = await mediator.Send(new UpdateExampleCommand(dto), cancellationToken);
        return result.ToActionResult().ToMvcResult();
    }
    public static async Task<IResult> DeleteExample(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        Result result = await mediator.Send(new DeleteExampleCommand(id), cancellationToken);
        return result.ToActionResult().ToMvcResult();
    }
    #endregion

    #region Interfaces
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/example");
        group.MapGet("/", GetAllExamples)
            .WithName(nameof(GetAllExamples))
            .Produces<IEnumerable<ExampleDto>>(StatusCodes.Status200OK);
        group.MapGet("/{id:guid}", GetExampleById)
            .WithName(nameof(GetExampleById))
            .Produces<ExampleDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        group.MapPost("/", CreateExample)
            .WithName(nameof(CreateExample))
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        group.MapPut("/", UpdateExample)
            .WithName(nameof(UpdateExample))
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        group.MapDelete("/{id:guid}", DeleteExample)
            .WithName(nameof(DeleteExample))
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        return builder;
    }
    #endregion
}