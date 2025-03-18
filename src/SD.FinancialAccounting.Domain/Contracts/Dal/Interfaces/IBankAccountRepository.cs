using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;

namespace SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;

public interface IBankAccountRepository: IDbRepository
{
    public Task<IReadOnlyList<BankAccountEntity>> QueryAllAccounts(CancellationToken cancellationToken);

    public Task<IReadOnlyList<BankAccountEntity>> QueryAccountsByIds(long[] ids, CancellationToken cancellationToken);

    public Task<long[]> AddAccounts(BankAccountEntity[] entities, CancellationToken cancellationToken);

    public Task DeleteAccount(long id, CancellationToken cancellationToken);

    public Task<long[]> UpdateAccountsBalance(BankAccountEntity[] entities, CancellationToken cancellationToken);

    public Task<long[]> UpdateAccountName(BankAccountEntity[] entities, CancellationToken cancellationToken);
}