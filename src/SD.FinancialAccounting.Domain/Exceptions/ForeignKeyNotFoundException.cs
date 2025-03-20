namespace SD.FinancialAccounting.Domain.Exceptions;

public class ForeignKeyNotFoundException: EntityNotFoundException
{
    public ForeignKeyNotFoundException()
    {
    }

    public ForeignKeyNotFoundException(string? message) : base(message)
    {
    }

    public ForeignKeyNotFoundException(string? message, EntityNotFoundException? innerException) : base(message, innerException)
    {
    }
}