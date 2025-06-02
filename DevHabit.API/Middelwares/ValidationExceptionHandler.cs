using Microsoft.AspNetCore.Diagnostics;

namespace DevHabit.API.Middelwares;

public sealed class ValidationExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        if(exception is not ValidationException validationException)
        {
            return false;
        }

        var erros = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key.ToLowerInvariant(),
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        ProblemDetailsContext problemDetailsContext = new()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Title = "Validation Server Error",
                Detail = "An Validation error occurred.",
                Status = StatusCodes.Status400BadRequest,
            }
        };

        problemDetailsContext.ProblemDetails.Extensions.Add("errors", erros);



        return await problemDetailsService.TryWriteAsync(problemDetailsContext);

    }
}
