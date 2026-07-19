using Mediator;
using FluentResults;
using PhoenixKC.WebAPI.Features.Example.Dtos;

namespace PhoenixKC.WebAPI.Features.Example.Handlers;

public sealed record class UpdateExampleCommand(ExampleDto Example) : IRequest<Result>; 