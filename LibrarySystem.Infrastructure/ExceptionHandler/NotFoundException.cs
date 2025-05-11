namespace LibrarySystem.Infrastructure.ExceptionHandler
{
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
