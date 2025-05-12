namespace LibrarySystem.Infrastructure.ExceptionHandler
{

    public class InUseException : AppException
    {
        public InUseException(string message, string userFriendlyMessage = null, string errorCode = null)
            : base(message, userFriendlyMessage ?? "this parameter is in Use", errorCode)
        {

        }
    }
}
