using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Exceptions;
using SD.FinancialAccounting.Domain.Export;
using SD.FinancialAccounting.Domain.Export.Enums;
using SD.FinancialAccounting.Domain.Mappers;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Domain.Services;

internal sealed class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountService(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<BankAccountModel> CreateBankAccount(string name, CancellationToken cancellationToken)
    {
        ValidateAccountName(name);
        using var transaction = _bankAccountRepository.CreateTransactionScope();

        var createdEntity = (await _bankAccountRepository.AddAccounts(
            entities:
            [
                new BankAccountEntity
                {
                    Name = name,
                    Balance = 0
                }
            ],
            cancellationToken: cancellationToken))[0];

        transaction.Complete();

        return createdEntity.MapEntityToModel();
    }

    public async Task<BankAccountModel> EditBankAccount(BankAccountModel newModel, CancellationToken cancellationToken)
    {
        try
        {
            return await EditBankAccountUnsafe(newModel, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new AccountNotFoundException($"Account with id : {newModel.Id} not found.", ex);
        }
    }

    private async Task<BankAccountModel> EditBankAccountUnsafe(BankAccountModel newModel,
        CancellationToken cancellationToken)
    {
        ValidateAccountName(newModel.Name);

        using var transaction = _bankAccountRepository.CreateTransactionScope();
        
        var updatedEntity =
            (await _bankAccountRepository.UpdateAccountName(entities:
                [
                    new BankAccountEntity
                    {
                        Id = newModel.Id,
                        Name = newModel.Name
                    }
                ],
                cancellationToken: cancellationToken))[0];
        
        transaction.Complete();

        return updatedEntity.MapEntityToModel();
    }

    public async Task DeleteBankAccount(long id, CancellationToken cancellationToken)
    {
        try
        {
            await _bankAccountRepository.DeleteAccount(id: id, cancellationToken: cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new AccountNotFoundException($"Account with id : {id} not found.", ex);
        }
    }

    public async Task<IReadOnlyList<BankAccountModel>> GetAllAccounts(CancellationToken cancellationToken)
    {
        var entities = await _bankAccountRepository.QueryAllAccounts(cancellationToken);

        return entities.MapEntitiesToModels();
    }

    public async Task ExportAccounts(ExportType exportType, string exportPath, CancellationToken cancellationToken)
    {
        var operations = await GetAllAccounts(cancellationToken);

        DataExporter exporter = DataExporter.Instance;
        
        await exporter.ExportAsync(
            models: operations,
            exportType: exportType,
            exportPath: exportPath,
            cancellationToken: cancellationToken
        );
    }

    private static void ValidateAccountName(string accountName)
    {
        if (accountName.Length > 50)
        {
            throw new ValidationException("Account name must be at most 50 characters long");
        }

        if (accountName.Length < 5)
        {
            throw new ValidationException("Account name must be at least 5 characters long");
        }
    }
}