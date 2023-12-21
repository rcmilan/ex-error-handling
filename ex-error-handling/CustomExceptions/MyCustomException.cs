using Microsoft.AspNetCore.Mvc;

namespace ex_error_handling.CustomExceptions
{
    public class MyCustomException(string message) : Exception(message)
    {
        public async Task Handle(HttpContext httpContext, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status417ExpectationFailed;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.14",
                Detail = message
            }, cancellationToken);
        }
    }
}
