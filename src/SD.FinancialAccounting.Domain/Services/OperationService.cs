using SD.FinancialAccounting.Domain.Containers;
using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Exceptions;
using SD.FinancialAccounting.Domain.Export;
using SD.FinancialAccounting.Domain.Export.Enums;
using SD.FinancialAccounting.Domain.Mappers;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Domain.Services;

internal sealed class OperationService : IOperationService
{
    private readonly IOperationsRepository _operationsRepository;
    private readonly IBalanceService _balanceService;

    public OperationService(IOperationsRepository operationsRepository, IBalanceService balanceService)
    {
        _operationsRepository = operationsRepository;
        _balanceService = balanceService;
    }

    public async Task<long> CreateOperation(CreateOperationContainer container, CancellationToken cancellationToken)
    {
        try
        {
            return await CreateOperationUnsafe(container, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ForeignKeyNotFoundException("Associated account or category id not found.", ex);
        }
    }

    private async Task<long> CreateOperationUnsafe(CreateOperationContainer container,
        CancellationToken cancellationToken)
    {
        ValidateOperationAmount(container.Amount);
        ValidateOperationDescription(container.Description);

        using var transaction = _operationsRepository.CreateTransactionScope();

        long[] createdOperationId = await _operationsRepository.AddOperations(
            entities:
            [
                new OperationEntity
                {
                    Amount = container.Amount,
                    Description = container.Description,
                    BankAccountId = container.BankAccountId,
                    CategoryId = container.OperationCategoryId,
                    Date = container.Date
                }
            ],
            cancellationToken: cancellationToken
        );

        await _balanceService.UpdateAccountsBalances([container.BankAccountId], cancellationToken);

        transaction.Complete();

        return createdOperationId[0];
    }

    public async Task EditOperationDescription(long id, string newDescription,
        CancellationToken cancellationToken)
    {
        try
        {
            ValidateOperationDescription(newDescription);
            using var transaction = _operationsRepository.CreateTransactionScope();
            var editedEntity =
                await _operationsRepository.UpdateOperationDescription(id, newDescription, cancellationToken);
            transaction.Complete();
        }
        catch (EntityNotFoundException ex)
        {
            throw new OperationNotFoundException($"Operation with id: {id} not found", ex);
        }
    }

    public async Task<EditedOperationContainer> EditOperationAmount(long id, decimal newAmount,
        CancellationToken cancellationToken)
    {
        try
        {
            ValidateOperationAmount(newAmount);
            using var transaction = _operationsRepository.CreateTransactionScope();

            var editedEntity =
                await _operationsRepository.UpdateOperationAmount(id, newAmount, cancellationToken);

            await _balanceService.UpdateAccountsBalances([editedEntity.BankAccountId], cancellationToken);

            transaction.Complete();
            return new EditedOperationContainer(
                AffectedAccountId: new List<long> { editedEntity.BankAccountId }
            );
        }
        catch (EntityNotFoundException ex)
        {
            throw new OperationNotFoundException($"Operation with id: {id} not found", ex);
        }
    }

    public async Task<EditedOperationContainer> EditOperationBankAccountId(long id, long newAccountId,
        CancellationToken cancellationToken)
    {
        try
        {
            using var transaction = _operationsRepository.CreateTransactionScope();

            var oldEntity = await _operationsRepository.QueryOperationById(id, cancellationToken);
            if (oldEntity.BankAccountId == newAccountId)
            {
                throw new ValidationException("New bank account id must be different from previous value.");
            }

            var editedEntity =
                await _operationsRepository.UpdateOperationAccountId(id, newAccountId, cancellationToken);

            await _balanceService.UpdateAccountsBalances([editedEntity.BankAccountId, oldEntity.BankAccountId],
                cancellationToken);

            transaction.Complete();
            return new EditedOperationContainer(
                AffectedAccountId: new List<long> { oldEntity.BankAccountId, editedEntity.BankAccountId }
            );
        }
        catch (ForeignKeyNotFoundException)
        {
            throw;
        }
        catch (EntityNotFoundException ex)
        {
            throw new OperationNotFoundException($"Operation with id: {id} not found", ex);
        }
    }

    public async Task<EditedOperationContainer> EditOperationCategoryId(long id, long newCategoryId,
        CancellationToken cancellationToken)
    {
        try
        {
            using var transaction = _operationsRepository.CreateTransactionScope();
            var editedEntity =
                await _operationsRepository.UpdateOperationCategoryId(id, newCategoryId, cancellationToken);

            await _balanceService.UpdateAccountsBalances([editedEntity.BankAccountId], cancellationToken);

            transaction.Complete();
            return new EditedOperationContainer(
                AffectedAccountId: new List<long> { editedEntity.BankAccountId }
            );
        }
        catch (ForeignKeyNotFoundException)
        {
            throw;
        }
        catch (EntityNotFoundException ex)
        {
            throw new OperationNotFoundException($"Operation with id: {id} not found", ex);
        }
    }

    public async Task<long> DeleteOperation(long id, CancellationToken cancellationToken)
    {
        try
        {
            using var transaction = _operationsRepository.CreateTransactionScope();
            var deletedEntity = await _operationsRepository.DeleteOperation(id, cancellationToken);

            await _balanceService.UpdateAccountsBalances([deletedEntity.BankAccountId], cancellationToken);
            transaction.Complete();

            return deletedEntity.BankAccountId;
        }
        catch (EntityNotFoundException ex)
        {
            throw new OperationNotFoundException($"Operation with id: {id} not found", ex);
        }
    }

    public async Task<IReadOnlyList<OperationModel>> GetOperations(CancellationToken cancellationToken)
    {
        var operationViewEntities = await _operationsRepository.QueryOperations(cancellationToken);

        return operationViewEntities.MapViewEntitiesToModels();
    }

    public async Task<IReadOnlyList<OperationModel>> GetOperationByBankAccountId(long bankAccountId,
        CancellationToken cancellationToken)
    {
        var operationViewEntities =
            await _operationsRepository.QueryOperationsByAccounts([bankAccountId], cancellationToken);

        return operationViewEntities.MapViewEntitiesToModels();
    }

    public async Task ExportOperations(ExportType exportType, string exportPath, CancellationToken cancellationToken)
    {
        IReadOnlyList<OperationModel> operations = await GetOperations(cancellationToken);

        DataExporter exporter = DataExporter.Instance;
        
        await exporter.ExportAsync(
            models: operations,
            exportType: exportType,
            exportPath: exportPath,
            cancellationToken: cancellationToken
        );
    }

    private static void ValidateOperationDescription(string description)
    {
        if (description.Length > 255)
        {
            throw new ValidationException("Operation description must be at most 255 characters long.");
        }
    }

    private static void ValidateOperationAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ValidationException("Operation amount must be greater than zero");
        }
    }
}