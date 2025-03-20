using SD.FinancialAccounting.Domain.Containers;
using SD.FinancialAccounting.Domain.Export.Enums;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Models.Enums;

namespace SD.FinancialAccounting.Domain.Services.Interfaces;

public interface IOperationCategoryService
{
    public Task<OperationCategoryModel> CreateCategory(OperationCategoryModel model, CancellationToken cancellationToken);

    // Edit methods are separated due to significant differences in the logic of change processing.
    public Task<OperationCategoryModel> EditCategoryName(long id, string newName, CancellationToken cancellationToken);

    public Task<EditedCategoryContainer> EditCategoryType(long id, OperationCategoryType newType,
        CancellationToken cancellationToken);

    public Task<IReadOnlyList<long>> DeleteCategory(long id, CancellationToken cancellationToken);

    public Task<IReadOnlyList<OperationCategoryModel>> GetAllCategories(CancellationToken cancellationToken);
    
    public Task ExportCategories(ExportType exportType, string exportPath, CancellationToken cancellationToken);
}