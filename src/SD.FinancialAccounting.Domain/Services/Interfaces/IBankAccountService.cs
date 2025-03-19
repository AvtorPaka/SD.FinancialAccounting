using SD.FinancialAccounting.Domain.Export.Enums;
using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Domain.Services.Interfaces;

public interface IBankAccountService
{
    public Task<BankAccountModel> CreateBankAccount(string name, CancellationToken cancellationToken);

    public Task<BankAccountModel> EditBankAccount(BankAccountModel newModel, CancellationToken cancellationToken);

    public Task DeleteBankAccount(long id, CancellationToken cancellationToken);

    public Task<IReadOnlyList<BankAccountModel>> GetAllAccounts(CancellationToken cancellationToken);

    public Task ExportAccounts(ExportType exportType, string exportPath, CancellationToken cancellationToken);
}