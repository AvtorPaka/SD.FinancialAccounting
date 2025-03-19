namespace SD.FinancialAccounting.Console.Contracts.Requests.Analytics;

public record GetAccountOperationsGroupedRequest(
    long AccountId
) : IRequest;