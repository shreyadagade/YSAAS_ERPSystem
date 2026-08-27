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


        private static async Task
            HandleExceptionAsync(
                HttpContext context,
                Exception exception)
        {
            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;


            var response =
                new
                {
                    statusCode = 500,

                    message =
                        exception.Message
                };


            await context.Response.WriteAsync(
                JsonSerializer.Serialize(
                    response));
        }
    }
}