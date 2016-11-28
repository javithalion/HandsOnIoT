using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;

namespace Javithalion.IoT.DeviceEvents.Service.Middlewares
{
    public class ErrorHandlingMiddleware
    {      
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;            
            _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, $"Error when processing request from {GetRemoteIpAddress(context)} with id {context.TraceIdentifier}");
                await HandleExceptionAsync(context, ex);
            }
        }

        public string GetRemoteIpAddress(HttpContext context)
        {
            return context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null) return;

            var code = HttpStatusCode.InternalServerError;

            await WriteExceptionAsync(context, exception, code).ConfigureAwait(false);
        }

        private static async Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;
            await response.WriteAsync(JsonConvert.SerializeObject(new
            {
                error = new
                {
                    message = exception.Message,
                    exception = exception.GetType().Name
                }
            })).ConfigureAwait(false);
        }
    }
}
