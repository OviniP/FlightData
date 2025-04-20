using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;


namespace FlightData.Api.Middleware
{
    /// <summary>
    /// Handles unhandled exceptions globally and returns a structured <see cref="ProblemDetails"/> response.
    /// </summary>
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log exception details.</param>
        /// <param name="env">The hosting environment, used to determine whether to show stack traces.</param>
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Attempts to handle an unhandled exception by returning a standardized <see cref="ProblemDetails"/> response.
        /// </summary>
        /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
        /// <param name="exception">The unhandled exception that occurred.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Returns <c>true</c> if the exception was successfully handled.</returns>
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error"
            };
            if (_env.IsDevelopment())
            {
                problemDetails.Detail = exception.StackTrace; // Include stack trace in development
                problemDetails.Detail += "\n\n" + exception.Message; // Include message in development
            }
            else
            {
                switch (exception)
                {
                    case ArgumentNullException argNullEx:
                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Detail = "A required parameter was missing.";
                        break;

                    case UnauthorizedAccessException unauthEx:
                        problemDetails.Status = StatusCodes.Status401Unauthorized;
                        problemDetails.Detail = "Unauthorized access.";
                        break;

                    case KeyNotFoundException notFoundEx:
                        problemDetails.Status = StatusCodes.Status404NotFound;
                        problemDetails.Detail = "Resource not found.";
                        break;

                    case IOException ioEx:
                        problemDetails.Detail = "A file or stream error occurred. Please try again later.";
                        break;

                    default:
                        problemDetails.Detail = "An unexpected error occurred. Please try again later."; // Generic message in production
                        break;
                }
               
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
