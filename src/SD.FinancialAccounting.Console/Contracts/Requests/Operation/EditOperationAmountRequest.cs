namespace SD.FinancialAccounting.Console.Contracts.Requests.Operation;

public record EditOperationAmountRequest(
    long Id,
    decimal NewAmount
);