namespace SD.FinancialAccounting.Domain.Exceptions;

public class AccountNotFoundException: EntityNotFoundException
{

    public AccountNotFoundException(string? message, EntityNotFoundException? innerException) : base(message, innerException)
    {
    }
}