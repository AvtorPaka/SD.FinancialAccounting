namespace SD.FinancialAccounting.Domain.Models;

public record BankAccountModel(
    long Id,
    string Name,
    decimal Balance
)
{
    public override string ToString()
    {
        return $"Id : {Id}\nName : {Name}\nCurrent balance : {Balance}";
    }
};