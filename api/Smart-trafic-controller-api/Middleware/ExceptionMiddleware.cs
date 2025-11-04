using System.Net;
using System.Text.Json;
using Smart_traffic_controller_api.Mappers;

namespace Smart_traffic_controller_api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string logMessage = ExceptionMessageMapper.GetLogMessage(ex);
                var (responseMessage, statusCode) = ExceptionMessageMapper.GetExceptionDetails(ex);

                _logger.LogError(ex, "An exception has occurred: {0}", logMessage);
                await HandleExceptionAsync(context, responseMessage, statusCode)
                    .ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context,
            string responseMessage,
            HttpStatusCode statusCode
        )
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            string responseJson = JsonSerializer.Serialize(new { Message = responseMessage });
            return context.Response.WriteAsync(responseJson);
        }
    }
}
