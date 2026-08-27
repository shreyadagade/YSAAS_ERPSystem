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
            catch (EmailException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Registration email could not be sent. Request: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await WriteResponseAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Application exception occurred. Request: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await WriteResponseAsync(
                    context,
                    ex.StatusCode,
                    ex.Message);
            }
            catch (SqlException ex)
            {
                _logger.LogError(
                    ex,
                    "Database exception occurred. Request: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await WriteResponseAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Validation exception occurred. Request: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await WriteResponseAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Invalid operation occurred. Request: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await WriteResponseAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred. Request: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await WriteResponseAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Something went wrong while processing your request. Please try again later.");
            }
        }

        private static async Task WriteResponseAsync(
            HttpContext context,
            int statusCode,
            string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                statusCode,
                message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}