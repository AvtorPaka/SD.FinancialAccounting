namespace SD.FinancialAccounting.Domain.Exceptions;

public class OperationNotFoundException: EntityNotFoundException
{
    public OperationNotFoundException()
    {
    }

    public OperationNotFoundException(string? message) : base(message)
    {
    }

    public OperationNotFoundException(string? message, EntityNotFoundException? innerException) : base(message, innerException)
    {
    }
}