namespace LibrarySystem.Infrastructure.ExceptionHandler
{
    // For business rule violations
    public class BusinessRuleException : AppException
    {
        public BusinessRuleException(string message, string userFriendlyMessage = null, string errorCode = null)
            : base(message, userFriendlyMessage ?? "A business rule was violated.", errorCode)
        {
        }
    }
}
