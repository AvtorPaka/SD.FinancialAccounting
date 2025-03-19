using SD.FinancialAccounting.Console.Contracts.Requests.Analytics;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Console.Controllers;

public class AnalyticsController
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public async Task<ControllerResponse> GetAccountOperationsPeriodDifference(
        AccountOperationsPeriodDifferenceRequest request, CancellationToken cancellationToken)
    {
        decimal difference = await _analyticsService.CalculateOperationsDifferenceForPeriod(
            accountId: request.AccountId,
            startDate: request.StartDate,
            endDate: request.EndDate,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: $"Incomes and expenses differences for account with id: {request.AccountId}: {difference}"
        );
    }

    public async Task<ControllerResponse> GetAccountOperationsGrouped(long accountId,
        CancellationToken cancellationToken)
    {
        AccountOperationsSummaryModel summaryModel = await _analyticsService.CalculateAccountOperationsGroup(
            accountId: accountId,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body:
            $"\nIncomes:\n{summaryModel.Incomes.ConvertToString()}\nExpenses:\n{summaryModel.Expenses.ConvertToString()}"
        );
    }
}