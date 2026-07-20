using Mediator;
using FluentResults;

namespace PhoenixKC.WebAPI.Features.Example.Handlers;

public sealed record class DeleteExampleCommand(Guid Id) : IRequest<Result>; 