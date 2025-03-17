namespace SD.FinancialAccounting.Console.Contracts.Requests.Operation;

public record EditOperationDescriptionRequest(
    long Id,
    string Description
);