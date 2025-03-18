using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Models.Enums;

namespace SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;

public interface ICategoriesRepository: IDbRepository
{
    public Task<IReadOnlyList<CategoryEntity>> AddCategories(CategoryEntity[] entities, CancellationToken cancellationToken);

    public Task<IReadOnlyList<CategoryEntity>> QueryAllCategories(CancellationToken cancellationToken);

    public Task<CategoryEntity> UpdateCategoryName(long id, string newName, CancellationToken cancellationToken);

    public Task<CategoryEntity> UpdateCategoryType(long id, OperationCategoryType newType, CancellationToken cancellationToken);

    public Task DeleteCategory(long id, CancellationToken cancellationToken);
}