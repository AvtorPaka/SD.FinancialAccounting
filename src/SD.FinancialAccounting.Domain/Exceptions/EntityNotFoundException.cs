namespace SD.FinancialAccounting.Domain.Exceptions;

public class EntityNotFoundException: Exception
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string? message) : base(message)
    {
    }

    protected EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}