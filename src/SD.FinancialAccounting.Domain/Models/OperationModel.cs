using SD.FinancialAccounting.Domain.Models.Interfaces;

namespace SD.FinancialAccounting.Domain.Models;

public record OperationModel(
    long Id,
    long BankAccountId,
    OperationCategoryModel Category,
    decimal Amount,
    DateTimeOffset Date,
    string Description
): IExportable
{
    public override string ToString()
    {
        return
            $"Id : {Id}\nBank account Id : {BankAccountId}\nAmount: {Amount}\nDate : {Date}\nDescription : {Description}\n\nCategory :\n{Category}";
    }
};