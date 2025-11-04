using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Smart_traffic_controller_api.Mappers
{
    public class ExceptionMessageMapper
    {
        private static readonly Dictionary<
            Type,
            (string responseMessage, HttpStatusCode responseStatusCode)
        > ExceptionMessages = new Dictionary<Type, (string, HttpStatusCode)>
        {
            {
                typeof(ArgumentException),
                ("An argument failed validation", HttpStatusCode.BadRequest)
            },
            {
                typeof(DbUpdateException),
                ("Internal server error.", HttpStatusCode.InternalServerError)
            },
            {
                typeof(UnauthorizedAccessException),
                ("Invalid credentials.", HttpStatusCode.Unauthorized)
            },
            {
                typeof(ArgumentNullException),
                ("A required argument was null.", HttpStatusCode.BadRequest)
            },
            {
                typeof(KeyNotFoundException),
                ("The requested resource was not found.", HttpStatusCode.NotFound)
            },
            {
                typeof(InvalidOperationException),
                ("An invalid operation was attempted.", HttpStatusCode.BadRequest)
            },
        };

        internal static (
            string responseMessage,
            HttpStatusCode responseStatusCode
        ) GetExceptionDetails(Exception exception)
        {
            return ExceptionMessages.ContainsKey(exception.GetType())
                ? ExceptionMessages[exception.GetType()]
                : ("An unexpected error occurred.", HttpStatusCode.InternalServerError);
        }

        internal static string GetLogMessage(Exception exception)
        {
            return exception.Message;
        }
    }
}
