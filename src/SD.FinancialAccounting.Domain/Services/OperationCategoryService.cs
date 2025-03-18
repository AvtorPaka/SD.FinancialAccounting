using SD.FinancialAccounting.Domain.Containers;
using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Exceptions;
using SD.FinancialAccounting.Domain.Mappers;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Models.Enums;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Domain.Services;

internal sealed class OperationCategoryService : IOperationCategoryService
{
    private readonly ICategoriesRepository _categoriesRepository;

    public OperationCategoryService(ICategoriesRepository categoriesRepository)
    {
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

    // TODO: Add tough shit
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
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
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
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<OperationCategoryModel>> GetAllCategories(CancellationToken cancellationToken)
    {
        var categoryEntities = await _categoriesRepository.QueryAllCategories(cancellationToken);

        return categoryEntities.MapEntitiesToModels();
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