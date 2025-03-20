using SD.FinancialAccounting.Domain.Models.Interfaces;

namespace SD.FinancialAccounting.Domain.Models;

public record BankAccountModel(
    long Id,
    string Name,
    decimal Balance
): IExportable
{
    public override string ToString()
    {
        return $"Id : {Id}\nName : {Name}\nCurrent balance : {Balance}";
    }
};