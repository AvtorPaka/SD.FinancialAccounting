using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Domain.Services.Interfaces;

public interface IAnalyticsService
{
    public Task<decimal> CalculateOperationsDifferenceForPeriod(long accountId, DateTimeOffset startDate,
        DateTimeOffset endDate, CancellationToken cancellationToken);

    public Task<AccountOperationsSummaryModel> CalculateAccountOperationsGroup(long accountId,
        CancellationToken cancellationToken);
}