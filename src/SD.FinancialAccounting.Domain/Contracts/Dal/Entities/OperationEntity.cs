namespace SD.FinancialAccounting.Domain.Contracts.Dal.Entities;

public class OperationEntity
{
    public long Id { get; init; }
    public long BankAccountId { get; init; }
    public long CategoryId { get; init; }
    public decimal Amount { get; init; }
    public DateTimeOffset Date { get; init; }
    public string Description { get; init; } = string.Empty;
}