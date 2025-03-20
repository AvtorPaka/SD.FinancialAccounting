namespace SD.FinancialAccounting.Domain.Exceptions;

public class ExportException: Exception
{
    public ExportException()
    {
    }

    public ExportException(string? message) : base(message)
    {
    }

    public ExportException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}