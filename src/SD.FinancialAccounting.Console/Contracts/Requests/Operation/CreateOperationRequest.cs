namespace SD.FinancialAccounting.Console.Contracts.Requests.Operation;

public record CreateOperationRequest(
    long BankAccountId,
    long OperationCategoryId,
    decimal Amount,
    string? Description
);