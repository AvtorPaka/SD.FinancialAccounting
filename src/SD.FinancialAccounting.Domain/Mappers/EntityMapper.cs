using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Domain.Mappers;

internal static class EntityMapper
{
    internal static BankAccountModel MapEntityToModel(this BankAccountEntity entity)
    {
        return new BankAccountModel(
            Id: entity.Id,
            Name: entity.Name,
            Balance: entity.Balance
        );
    }

    internal static IReadOnlyList<BankAccountModel> MapEntitiesToModels(this IReadOnlyList<BankAccountEntity> entities)
    {
        return entities.Select(e => e.MapEntityToModel()).ToList();
    }

    internal static OperationCategoryModel MapEntityToModel(this CategoryEntity entity)
    {
        return new OperationCategoryModel(
            Id: entity.Id,
            Type: entity.Type,
            Name: entity.Name
        );
    }

    internal static IReadOnlyList<OperationCategoryModel> MapEntitiesToModels(
        this IReadOnlyList<CategoryEntity> entities)
    {
        return entities.Select(e => e.MapEntityToModel()).ToList();
    }
}