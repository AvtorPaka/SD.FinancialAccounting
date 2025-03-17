namespace SD.FinancialAccounting.Console.Contracts.Requests.Account;

public record EditAccountRequest(
    long Id,
    string NewName
);