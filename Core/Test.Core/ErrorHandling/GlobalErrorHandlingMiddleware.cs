using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Test.Core.Models.Data;
using Test.Core.Models.Exceptions;

namespace Test.Core.ErrorHandling
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
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

        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError; ;
            var stackTrace = String.Empty;
            var exceptionType = exception.GetType();

            var exceptionResult = JsonSerializer.Serialize(new ExceptionResponse()
            {

                Status = Status.Error,
                Error = new ErrorModel()
                {
                    Message = exception.Message?.ToString(),
                    StackTrace = exception.StackTrace?.ToString()
                }
            });

            _logger.LogError(exception, $"Details: {exception.Message}");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsync(exceptionResult);
        }
    }
}