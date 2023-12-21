using ex_error_handling.CustomExceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ex_error_handling.Handlers
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            switch (exception)
            {
                case MyCustomException e:
                    await HandleMyCustomException(httpContext, e, cancellationToken);
                    break;
                default:
                    await HandleDefault(httpContext, cancellationToken);
                    break;
            }

            var exceptionMessage = exception.Message;

            logger.LogError("TraceId {traceId} - Message: {exceptionMessage}", httpContext.TraceIdentifier, exceptionMessage);

            return true;
        }

        private static async Task HandleDefault(HttpContext httpContext, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status501NotImplemented;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2"
            }, cancellationToken);
        }

        private static async Task HandleMyCustomException(HttpContext httpContext, MyCustomException exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status417ExpectationFailed;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.14",
                Detail = exception.Message
            }, cancellationToken);
        }
    }
}
