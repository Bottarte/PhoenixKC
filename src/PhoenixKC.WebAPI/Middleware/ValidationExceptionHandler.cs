using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics;

namespace PhoenixKC.WebAPI.Middleware;

[ExcludeFromCodeCoverage]
public sealed class ValidationExceptionHandler(IProblemDetailsService thisProblemDetails) : IExceptionHandler
{
    #region Interfaces
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if(exception is ValidationException validation_exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            ValidationProblemDetails problem = new(
                validation_exception.Errors.GroupBy(e => e.PropertyName).ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                )
            )
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed"
            };
            if(problem.Errors.Count == 0)
            {
                problem.Detail = validation_exception.Message;
            }
            await thisProblemDetails.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = problem
            });
            return true;
        }
        return false;
    }
    #endregion
}