namespace SD.FinancialAccounting.Domain.Contracts.Dal.Entities;

public class BankAccountEntity
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Balance { get; init; }
}