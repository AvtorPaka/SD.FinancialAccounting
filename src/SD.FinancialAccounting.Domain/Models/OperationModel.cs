namespace SD.FinancialAccounting.Domain.Models;

public record OperationModel(
    long Id,
    long BankAccountId,
    OperationCategoryModel Category,
    decimal Amount,
    DateTimeOffset Date,
    string Description
)
{
    public override string ToString()
    {
        return
            $"Id : {Id}\nBank account Id : {BankAccountId}\n Operation Category :\n{Category}\n\nDate : {Date}\nDescription : {Description}";
    }
};