
using System.Net;
using System.Text.Json;

namespace StudentManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(
                    context,
                    ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType =
                "application/json";

            int statusCode;

            switch (exception)
            {
                // =================================================
                // 400 - BAD REQUEST
                // =================================================

                case ArgumentException:
                    statusCode =
                        (int)HttpStatusCode.BadRequest;
                    break;

                // =================================================
                // 404 - NOT FOUND
                // =================================================

                case KeyNotFoundException:
                    statusCode =
                        (int)HttpStatusCode.NotFound;
                    break;

                // =================================================
                // 401 - UNAUTHORIZED
                // =================================================

                case UnauthorizedAccessException:
                    statusCode =
                        (int)HttpStatusCode.Unauthorized;
                    break;

                // =================================================
                // 409 - CONFLICT
                // =================================================

                case InvalidOperationException:
                    statusCode =
                        (int)HttpStatusCode.Conflict;
                    break;

                // =================================================
                // 500 - INTERNAL SERVER ERROR
                // =================================================

                default:
                    statusCode =
                        (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.StatusCode =
                statusCode;

            var response = new
            {
                statusCode = statusCode,

                message = exception.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
