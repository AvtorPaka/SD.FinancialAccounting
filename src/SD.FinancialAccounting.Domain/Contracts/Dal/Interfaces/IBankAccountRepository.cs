using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;

namespace SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;

public interface IBankAccountRepository: IDbRepository
{
    public Task<IReadOnlyList<BankAccountEntity>> QueryAllAccounts(CancellationToken cancellationToken);

    public Task<IReadOnlyList<BankAccountEntity>> QueryAccountsByIds(long[] ids, CancellationToken cancellationToken);

    public Task<IReadOnlyList<BankAccountEntity>> AddAccounts(BankAccountEntity[] entities, CancellationToken cancellationToken);

    public Task DeleteAccount(long id, CancellationToken cancellationToken);

    public Task<long[]> UpdateAccountsBalance(BankAccountEntity[] entities, CancellationToken cancellationToken);

    public Task<IReadOnlyList<BankAccountEntity>> UpdateAccountName(BankAccountEntity[] entities, CancellationToken cancellationToken);
}