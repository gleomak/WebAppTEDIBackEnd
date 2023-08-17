using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebAppTEDI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IWebHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate  next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }catch (SystemException ex) {
                logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                var response = new ProblemDetails
                {
                    Status = 500,
                    Detail = env.IsDevelopment() ? ex.StackTrace?.ToString() : null,
                    Title = ex.Message,
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
