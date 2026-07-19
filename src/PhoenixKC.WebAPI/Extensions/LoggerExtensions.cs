using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace PhoenixKC.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class LoggerExtensions
{
    [DoesNotReturn]
    public static void LogFailAndThrow<T>(this ILogger<T> logger, string messageTemplate, params object?[] args)
    {
        string formatted_message = string.Format(messageTemplate, args);
        logger.LogError(formatted_message);
        throw new ValidationException(formatted_message);
    }
}