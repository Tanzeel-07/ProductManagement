using System.Net.Mime;
using System.Net;
using System.Text.Json;
using ProductManagement.Services.Models;

namespace ProductManagement.WebApi.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> Logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                await HandleCustomExceptionResponseAsync(context, ex);
            }
        }

        private async Task HandleCustomExceptionResponseAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ErrorRS(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString());
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}
