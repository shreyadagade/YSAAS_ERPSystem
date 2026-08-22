using Microsoft.Data.SqlClient;
using UserManagement.Application.Exceptions;

namespace UserManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
            catch (AppException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Application exception occurred. Request: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    statusCode = ex.StatusCode,
                    message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (SqlException ex)
            {
                _logger.LogError(
                    ex,
                    "Database exception occurred. Request: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    message = "A database error occurred while processing your request."
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Validation exception occurred. Request: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode =
                    StatusCodes.Status400BadRequest;

                context.Response.ContentType = "application/json";

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred. Request: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    message = "Something went wrong while processing your request."
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}