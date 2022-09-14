using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;

namespace API.Middelware
{
    public class ExceptionMiddelware
    {
        public readonly RequestDelegate _next;
        public readonly ILogger<ExceptionMiddelware> _logger;
        public readonly IHostEnvironment _env;
        public ExceptionMiddelware(RequestDelegate next, ILogger<ExceptionMiddelware> logger, IHostEnvironment env)
        {
            this._env = env;
            this._logger = logger;
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiExceptions(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiExceptions(context.Response.StatusCode, "Internal server error");

                //Serialize response into JSON
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}