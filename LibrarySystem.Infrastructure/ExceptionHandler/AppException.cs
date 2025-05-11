using System;

namespace LibrarySystem.Infrastructure.ExceptionHandler
{
    public class AppException : Exception
    {
        public string UserFriendlyMessage { get; }
        public string ErrorCode { get; }

        public AppException(string message, string userFriendlyMessage = null, string errorCode = null)
            : base(message)
        {
            UserFriendlyMessage = userFriendlyMessage ?? "An error occurred while processing your request.";
            ErrorCode = errorCode;
        }

        public AppException(string message, Exception innerException, string userFriendlyMessage = null, string errorCode = null)
            : base(message, innerException)
        {
            UserFriendlyMessage = userFriendlyMessage ?? "An error occurred while processing your request.";
            ErrorCode = errorCode;
        }
    }
}
