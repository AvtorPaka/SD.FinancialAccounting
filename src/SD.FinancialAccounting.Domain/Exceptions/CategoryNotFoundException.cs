namespace SD.FinancialAccounting.Domain.Exceptions;

public class CategoryNotFoundException: EntityNotFoundException
{
    public CategoryNotFoundException(string? message) : base(message)
    {
        
    }
    
    public CategoryNotFoundException(string? message, EntityNotFoundException? innerException) : base(message, innerException)
    {
    }
}