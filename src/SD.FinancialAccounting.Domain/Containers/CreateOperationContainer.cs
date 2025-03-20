namespace SD.FinancialAccounting.Domain.Containers;

public record CreateOperationContainer(
    long BankAccountId,
    long OperationCategoryId,
    decimal Amount,
    string Description,
    DateTimeOffset Date
);