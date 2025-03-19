namespace SD.FinancialAccounting.Console.Contracts.Requests.Operation;

public record GetOperationsByAccountRequest(
    long BankAccountId
): IRequest;