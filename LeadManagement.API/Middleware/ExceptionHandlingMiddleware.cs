using System.Net;
using System.Text.Json;

namespace LeadManagement.API.Middleware
{
 


    
        public class ExceptionHandlingMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<ExceptionHandlingMiddleware> _logger;

            public ExceptionHandlingMiddleware(
                RequestDelegate next,
                ILogger<ExceptionHandlingMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (ArgumentException ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Validation error. Path: {Path}",
                        context.Request.Path);

                    await HandleExceptionAsync(
                        context,
                        HttpStatusCode.BadRequest,
                        ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Unhandled exception. Path: {Path}",
                        context.Request.Path);

                    await HandleExceptionAsync(
                        context,
                        HttpStatusCode.InternalServerError,
                        "An unexpected error occurred.");
                }
            }

            private static async Task HandleExceptionAsync(
                HttpContext context,
                HttpStatusCode statusCode,
                string message)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var response = new
                {
                    statusCode = (int)statusCode,
                    message = message
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }

    