using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Service.Infraestructure
{
    public class UnhandledExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //TODO :: Extend this

            string message = $"Internal exception: {context.Exception.Message}";
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";            
            response.WriteAsync(message);
        }        
    }
}
