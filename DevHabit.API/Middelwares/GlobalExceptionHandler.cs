using Microsoft.AspNetCore.Diagnostics;

namespace DevHabit.API.Middelwares;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    IHostEnvironment environment) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken)
    
    {
        return problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Title = "Internal Server Error",
                Detail = environment.IsDevelopment()
                    ? exception.Message
                    : "An unexpected error occurred. Please try again later."
            }

        });
    }
}
