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

            // =====================================================
            // 400 - BAD REQUEST
            // =====================================================
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

            // =====================================================
            // 404 - NOT FOUND
            // =====================================================
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Resource not found. Path: {Path}",
                    context.Request.Path);

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.NotFound,
                    ex.Message);
            }

            // =====================================================
            // 409 - CONFLICT
            // =====================================================
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Business conflict. Path: {Path}",
                    context.Request.Path);

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.Conflict,
                    ex.Message);
            }

            // =====================================================
            // 500 - INTERNAL SERVER ERROR
            // =====================================================
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception. Path: {Path}",
                    context.Request.Path);

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string message)
        {
            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                (int)statusCode;

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