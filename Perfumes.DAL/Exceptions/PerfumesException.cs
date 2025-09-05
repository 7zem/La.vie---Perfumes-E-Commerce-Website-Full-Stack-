namespace Perfumes.DAL.Exceptions
{
    public class PerfumesException : Exception
    {
        public PerfumesException() : base() { }
        public PerfumesException(string message) : base(message) { }
        public PerfumesException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class EntityNotFoundException : PerfumesException
    {
        public EntityNotFoundException(string entityName, object id) 
            : base($"{entityName} with id {id} was not found.") { }
    }

    public class DuplicateEntityException : PerfumesException
    {
        public DuplicateEntityException(string entityName, string field, object value) 
            : base($"{entityName} with {field} '{value}' already exists.") { }
    }

    public class InvalidOperationException : PerfumesException
    {
        public InvalidOperationException(string message) : base(message) { }
    }
} 