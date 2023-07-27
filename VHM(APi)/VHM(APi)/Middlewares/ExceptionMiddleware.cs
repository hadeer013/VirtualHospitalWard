using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using VHM_APi_.Errors;

namespace VHM_APi_.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment environment;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            this.environment = environment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError ;
                var response = environment.IsDevelopment() ?
                                 new ApiExceptionErrorResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                               : new ApiExceptionErrorResponse((int)HttpStatusCode.InternalServerError);
                var options= new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                
                var JsonExceptionResponse = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(JsonExceptionResponse);
            }
        }
    }
}
