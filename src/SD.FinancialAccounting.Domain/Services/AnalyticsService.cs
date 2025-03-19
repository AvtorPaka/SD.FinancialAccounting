using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Exceptions;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Models.Enums;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Domain.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IOperationsRepository _operationsRepository;

    public AnalyticsService(IOperationsRepository operationsRepository)
    {
        _operationsRepository = operationsRepository;
    }

    public async Task<decimal> CalculateOperationsDifferenceForPeriod(long accountId, DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<OperationViewEntity> accountOperations = await _operationsRepository.QueryOperationsByAccounts(
            accountIds: [accountId],
            cancellationToken: cancellationToken
        );

        if (accountOperations.Count == 0)
        {
            throw new OperationNotFoundException("Operations not found for this account.");
        }

        return accountOperations
            .Where(o => o.Date >= startDate && o.Date <= endDate)
            .Sum(o => o.CategoryType == OperationCategoryType.Income ? o.Amount : -o.Amount);
    }

    public async Task<AccountOperationsSummaryModel> CalculateAccountOperationsGroup(long accountId,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<OperationViewEntity> accountOperations = await _operationsRepository.QueryOperationsByAccounts(
            accountIds: [accountId],
            cancellationToken: cancellationToken
        );

        if (accountOperations.Count == 0)
        {
            throw new OperationNotFoundException("Operations not found for this account.");
        }

        var incomes = accountOperations
            .Where(o => o.CategoryType == OperationCategoryType.Income)
            .GroupBy(o => new OperationCategoryModel(
                Id: o.CategoryId,
                Type: o.CategoryType,
                Name: o.CategoryName
            ))
            .ToDictionary(
                g => g.Key,
                g => g.Sum(o => o.Amount)
            );


        var expenses = accountOperations
            .Where(o => o.CategoryType == OperationCategoryType.Expense)
            .GroupBy(o => new OperationCategoryModel(
                Id: o.CategoryId,
                Type: o.CategoryType,
                Name: o.CategoryName
            ))
            .ToDictionary(
                g => g.Key,
                g => g.Sum(o => o.Amount)
            );

        return new AccountOperationsSummaryModel(
            Incomes: incomes,
            Expenses: expenses,
            AccountId: accountId
        );
    }
}