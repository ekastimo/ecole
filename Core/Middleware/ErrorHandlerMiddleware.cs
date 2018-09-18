using System;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        //TODO use config to show more detailed errors
        private readonly IConfiguration _config;
        private readonly ILogger<ErrorHandlerMiddleware> _iLogger;

        public ErrorHandlerMiddleware(RequestDelegate next, IConfiguration config ,ILogger<ErrorHandlerMiddleware> iLogger)
        {
            _next = next;
            _config = config;
            _iLogger = iLogger;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            
            var showDetailedErrors = _config.GetValue<bool>("ShowDetailedErrors");
            _iLogger.LogError(exception,"global.error");
            var code = HttpStatusCode.InternalServerError; 

            var message = "Unknown error, please contact administrator";
            if (showDetailedErrors)
            {
                message = exception.StackTrace;
            }
            else
            {
                switch (exception)
                {
                    case NotFoundException _:
                        code = HttpStatusCode.NotFound;
                        message = "Oops, We cant find this resource";
                        break;
                    case ClientFriendlyException _:
                        code = HttpStatusCode.BadRequest;
                        message = exception.Message;
                        break;
                    
                    case UnauthorizedAccessException _:
                        code = HttpStatusCode.Unauthorized;
                        break;
                    case DbUpdateException _:
                    case SqlException _:
                        code = HttpStatusCode.BadRequest;
                        break;
                }
            }

            
            var result = JsonConvert.SerializeObject(new { Code = code, Message = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(result);
        }
    }
}
