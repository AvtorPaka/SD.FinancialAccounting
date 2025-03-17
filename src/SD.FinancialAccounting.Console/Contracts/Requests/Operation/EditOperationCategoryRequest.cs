namespace SD.FinancialAccounting.Console.Contracts.Requests.Operation;

public record EditOperationCategoryRequest(
    long Id,
    long NewOperationCategoryId
);