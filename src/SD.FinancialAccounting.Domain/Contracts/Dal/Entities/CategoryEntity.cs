using SD.FinancialAccounting.Domain.Models.Enums;

namespace SD.FinancialAccounting.Domain.Contracts.Dal.Entities;

public class CategoryEntity
{
    public long Id { get; init; }
    public OperationCategoryType Type { get; init; }
    public string Name { get; init; } = string.Empty;
}