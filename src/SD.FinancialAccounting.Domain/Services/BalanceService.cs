using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Models.Enums;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Domain.Services;

public class BalanceService : IBalanceService
{
    private readonly IOperationsRepository _operationsRepository;
    private readonly IBankAccountRepository _accountRepository;

    public BalanceService(IOperationsRepository operationsRepository, IBankAccountRepository accountRepository)
    {
        _operationsRepository = operationsRepository;
        _accountRepository = accountRepository;
    }

    public async Task UpdateAccountsBalances(long[] accountsIds, CancellationToken cancellationToken)
    {
        IReadOnlyList<OperationViewEntity> operations =
            await _operationsRepository.QueryOperationsByAccounts(accountsIds, cancellationToken);

        BankAccountEntity[] updatedAccounts;
        if (operations.Count != 0)
        {
            var balanceUpdates = operations
                .GroupBy(o => o.BankAccountId)
                .Select(g => new
                {
                    BankAccountId = g.Key,
                    NewBalance = g.Sum(o => o.CategoryType == OperationCategoryType.Income
                        ? o.Amount : -o.Amount)
                })
                .ToDictionary(x => x.BankAccountId, x => x.NewBalance);

            updatedAccounts = balanceUpdates
                .Select(kv => new BankAccountEntity
                {
                    Id = kv.Key,
                    Balance = kv.Value
                }).ToArray();
        }
        else
        {
            updatedAccounts = accountsIds
                .Select(id => new BankAccountEntity
                {
                    Id = id,
                    Balance = 0
                }).ToArray();
        }

        await _accountRepository.UpdateAccountsBalance(updatedAccounts, cancellationToken);
    }
}