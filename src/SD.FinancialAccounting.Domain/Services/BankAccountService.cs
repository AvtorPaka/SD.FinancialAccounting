using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Domain.Services;

internal sealed class BankAccountService: IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountService(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }
    
    public async Task<BankAccountModel> CreateBankAccount(string name, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<BankAccountModel> EditBankAccount(BankAccountModel newModel, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task DeleteBankAccount(long id, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<BankAccountModel>> GetAllAccounts(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}