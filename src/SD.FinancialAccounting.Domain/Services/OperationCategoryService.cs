using SD.FinancialAccounting.Domain.Containers;
using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Exceptions;
using SD.FinancialAccounting.Domain.Export;
using SD.FinancialAccounting.Domain.Export.Enums;
using SD.FinancialAccounting.Domain.Mappers;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Models.Enums;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Domain.Services;

internal sealed class OperationCategoryService : IOperationCategoryService
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IOperationsRepository _operationsRepository;
    private readonly IBalanceService _balanceService;

    public OperationCategoryService(ICategoriesRepository categoriesRepository,
        IOperationsRepository operationsRepository, IBalanceService balanceService)
    {
        _balanceService = balanceService;
        _operationsRepository = operationsRepository;
        _categoriesRepository = categoriesRepository;
    }

    public async Task<OperationCategoryModel> CreateCategory(OperationCategoryModel model,
        CancellationToken cancellationToken)
    {
        ValidateCategoryName(model.Name);
        using var transaction = _categoriesRepository.CreateTransactionScope();

        var createdEntity = await _categoriesRepository.AddCategories(
            entities:
            [
                new CategoryEntity
                {
                    Name = model.Name,
                    Type = model.Type
                }
            ],
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        return createdEntity[0].MapEntityToModel();
    }

    public async Task<OperationCategoryModel> EditCategoryName(long id, string newName,
        CancellationToken cancellationToken)
    {
        try
        {
            return await EditCategoryNameUnsafe(id, newName, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new CategoryNotFoundException($"Operation Category with id: {id} not found.", ex);
        }
    }

    private async Task<OperationCategoryModel> EditCategoryNameUnsafe(long id, string newName,
        CancellationToken cancellationToken)
    {
        ValidateCategoryName(newName);
        using var transaction = _categoriesRepository.CreateTransactionScope();

        var editedEntity = await _categoriesRepository.UpdateCategoryName(
            id: id,
            newName: newName,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        return editedEntity.MapEntityToModel();
    }

    public async Task<EditedCategoryContainer> EditCategoryType(long id, OperationCategoryType newType,
        CancellationToken cancellationToken)
    {
        try
        {
            return await EditCategoryTypeUnsafe(id, newType, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new CategoryNotFoundException($"Operation Category with id: {id} not found.", ex);
        }
    }

    private async Task<EditedCategoryContainer> EditCategoryTypeUnsafe(long id, OperationCategoryType newType,
        CancellationToken cancellationToken)
    {
        using var transaction = _categoriesRepository.CreateTransactionScope();

        var editedEntity = await _categoriesRepository.UpdateCategoryType(id, newType, cancellationToken);
        
        var operationViewEntities = await _operationsRepository.QueryOperations(cancellationToken);
        long[] affectedAccountsIds = operationViewEntities.Where(e => e.CategoryId == id).Select(e => e.BankAccountId).Distinct().ToArray();
        
        await _balanceService.UpdateAccountsBalances(affectedAccountsIds, cancellationToken);

        transaction.Complete();

        return new EditedCategoryContainer(
            EditedModel: editedEntity.MapEntityToModel(),
            AffectedAccountId: affectedAccountsIds
        );
    }

    public async Task<IReadOnlyList<long>> DeleteCategory(long id, CancellationToken cancellationToken)
    {
        try
        {
            return await DeleteCategoryUnsafe(id, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new CategoryNotFoundException($"Operation Category with id: {id} not found.", ex);
        }
    }

    private async Task<IReadOnlyList<long>> DeleteCategoryUnsafe(long id, CancellationToken cancellationToken)
    {
        using var transaction = _categoriesRepository.CreateTransactionScope();

        var operationViewEntities = await _operationsRepository.QueryOperations(cancellationToken);
        long[] affectedAccountsIds = operationViewEntities.Where(e => e.CategoryId == id).Select(e => e.BankAccountId).Distinct().ToArray();

        await _categoriesRepository.DeleteCategory(id, cancellationToken);

        await _balanceService.UpdateAccountsBalances(affectedAccountsIds, cancellationToken);

        transaction.Complete();

        return affectedAccountsIds.ToList();
    }

    public async Task<IReadOnlyList<OperationCategoryModel>> GetAllCategories(CancellationToken cancellationToken)
    {
        var categoryEntities = await _categoriesRepository.QueryAllCategories(cancellationToken);

        return categoryEntities.MapEntitiesToModels();
    }

    public async Task ExportCategories(ExportType exportType, string exportPath, CancellationToken cancellationToken)
    {
        var operations = await GetAllCategories(cancellationToken);

        DataExporter exporter = DataExporter.Instance;
        
        await exporter.ExportAsync(
            models: operations,
            exportType: exportType,
            exportPath: exportPath,
            cancellationToken: cancellationToken
        );
    }

    private static void ValidateCategoryName(string categoryName)
    {
        if (categoryName.Length < 5)
        {
            throw new ValidationException("Operation category name must be at lest 5 characters long");
        }

        if (categoryName.Length > 50)
        {
            throw new ValidationException("Operation category name must be at most 50 characters long");
        }
    }
}