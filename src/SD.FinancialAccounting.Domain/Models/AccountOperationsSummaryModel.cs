namespace SD.FinancialAccounting.Domain.Models;

public record AccountOperationsSummaryModel(
    long AccountId,
    Dictionary<OperationCategoryModel, decimal> Incomes,
    Dictionary<OperationCategoryModel, decimal> Expenses
);