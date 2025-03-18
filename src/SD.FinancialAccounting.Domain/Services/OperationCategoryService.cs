using SD.FinancialAccounting.Domain.Containers;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Models.Enums;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Domain.Services;

internal sealed class OperationCategoryService: IOperationCategoryService
{
    private readonly ICategoriesRepository _categoriesRepository;

    public OperationCategoryService(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }
    
    public async Task<OperationCategoryModel> CreateCategory(OperationCategoryModel model, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<OperationCategoryModel> EditCategoryName(long id, string newName, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<EditedCategoryContainer> EditCategoryType(long id, OperationCategoryType newType, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
    
    public async Task<IReadOnlyList<long>> DeleteCategory(long id, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<OperationCategoryModel>> GetAllCategories(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}