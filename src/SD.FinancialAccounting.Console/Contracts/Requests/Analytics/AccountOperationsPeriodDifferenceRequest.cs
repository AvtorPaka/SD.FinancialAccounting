namespace SD.FinancialAccounting.Console.Contracts.Requests.Analytics;

public record AccountOperationsPeriodDifferenceRequest(
    long AccountId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate
);