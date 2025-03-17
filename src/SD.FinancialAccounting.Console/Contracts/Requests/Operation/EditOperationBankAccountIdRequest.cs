namespace SD.FinancialAccounting.Console.Contracts.Requests.Operation;

public record EditOperationBankAccountIdRequest(
    long Id,
    long NewBankAccountId
);