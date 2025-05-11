using System;
using System.Collections.Generic;

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

    // For business rule violations
    public class BusinessRuleException : AppException
    {
        public BusinessRuleException(string message, string userFriendlyMessage = null, string errorCode = null)
            : base(message, userFriendlyMessage ?? "A business rule was violated.", errorCode)
        {
        }
    }

    // For validation failures
    public class ValidationException : AppException
    {
        public Dictionary<string, string> ValidationErrors { get; }

        public ValidationException(Dictionary<string, string> validationErrors)
            : base("Validation failed", "Please correct the validation errors and try again.")
        {
            ValidationErrors = validationErrors;
        }
    }

    // For unauthorized access
    public class UnauthorizedAccessException : AppException
    {
        public UnauthorizedAccessException(string message = "Access denied", string userFriendlyMessage = null)
            : base(message, userFriendlyMessage ?? "You don't have permission to access this resource.")
        {
        }
    }

    // For not found scenarios
    public class NotFoundException : AppException
    {
        public NotFoundException(string entityName, object entityId)
            : base($"{entityName} with id {entityId} not found",
                  $"{entityName} not found",
                  "NOT_FOUND")
        {
        }
    }
}
