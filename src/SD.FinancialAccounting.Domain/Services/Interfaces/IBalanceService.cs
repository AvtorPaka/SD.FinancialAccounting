namespace SD.FinancialAccounting.Domain.Services.Interfaces;

public interface IBalanceService
{
    public Task UpdateAccountsBalances(long[] accountsIds, CancellationToken cancellationToken);
}