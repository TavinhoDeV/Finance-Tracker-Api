namespace FinanceTracker.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entity, object key)
        : base($"Entity '{entity}' with key '{key}' was not found.") { }
}
