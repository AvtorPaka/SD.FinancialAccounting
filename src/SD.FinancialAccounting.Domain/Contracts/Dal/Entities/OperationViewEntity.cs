using SD.FinancialAccounting.Domain.Models.Enums;

namespace SD.FinancialAccounting.Domain.Contracts.Dal.Entities;

public class OperationViewEntity
{
    public long OperationId { get; init; }
    public long BankAccountId { get; init; }
    public long CategoryId { get; init; }
    public OperationCategoryType CategoryType { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTimeOffset Date { get; init; }
    public string Description { get; init; } = string.Empty;
}