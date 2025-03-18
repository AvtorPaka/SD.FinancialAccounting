using Npgsql;
using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;

namespace SD.FinancialAccounting.Infrastructure.Dal.Repositories;

public class BankAccountRepository: BaseRepository, IBankAccountRepository
{
    public BankAccountRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<IReadOnlyList<BankAccountEntity>> QueryAllAccounts(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<BankAccountEntity>> QueryAccountsByIds(long[] ids, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
    
    public async Task<long[]> AddAccounts(BankAccountEntity[] entities, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task DeleteAccount(long id, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<long[]> UpdateAccountsBalance(BankAccountEntity[] entities, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<long[]> UpdateAccountName(BankAccountEntity[] entities, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}