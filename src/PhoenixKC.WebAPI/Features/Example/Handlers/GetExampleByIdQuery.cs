using Mediator;
using FluentResults;
using PhoenixKC.WebAPI.Features.Example.Dtos;

namespace PhoenixKC.WebAPI.Features.Example.Handlers;

public sealed record class GetExampleByIdQuery(Guid Id) : IRequest<Result<ExampleDto>>;