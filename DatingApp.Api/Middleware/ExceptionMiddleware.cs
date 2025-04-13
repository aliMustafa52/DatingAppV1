using DatingApp.Api.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace DatingApp.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;
        private readonly IHostEnvironment _environment = environment;


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {

                _logger.LogError(exception,"Something went wrong: {Message}", exception.Message);

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _environment.IsDevelopment()
                    ? new ApiException(httpContext.Response.StatusCode, exception.Message, exception.StackTrace)
                    : new ApiException(httpContext.Response.StatusCode, exception.Message, "Internal server error");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var json = JsonSerializer.Serialize(response, options);
                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
