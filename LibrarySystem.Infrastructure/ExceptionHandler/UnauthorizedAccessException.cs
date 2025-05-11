namespace LibrarySystem.Infrastructure.ExceptionHandler
{
    // For unauthorized access
    public class UnauthorizedAccessException : AppException
    {
        public UnauthorizedAccessException(string message = "Access denied", string userFriendlyMessage = null)
            : base(message, userFriendlyMessage ?? "You don't have permission to access this resource.")
        {
        }
    }
}
