using System.Collections.Generic;

namespace LibrarySystem.Infrastructure.ExceptionHandler
{
    // For validation failures
    public class ValidationException : AppException
    {
        //public Dictionary<string, string> ValidationErrors { get; }

        public ValidationException(string message = null)
            : base("Validation failed", "Please correct the validation errors and try again.")
        {
            //ValidationErrors = validationErrors;
        }
    }
}
