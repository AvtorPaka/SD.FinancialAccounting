namespace SD.FinancialAccounting.Console.Contracts.Requests.Account;

public record DeleteAccountRequest(
    long Id
): IRequest;