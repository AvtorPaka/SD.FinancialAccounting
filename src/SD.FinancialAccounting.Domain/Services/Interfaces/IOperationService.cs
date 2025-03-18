using SD.FinancialAccounting.Domain.Containers;
using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Domain.Services.Interfaces;

public interface IOperationService
{
    public Task<long> CreateOperation(CreateOperationContainer container, CancellationToken cancellationToken);

    // Edit methods are separated due to significant differences in the logic of change processing.
    public Task<OperationModel> EditOperationDescription(long id, string newDescription, CancellationToken cancellationToken);

    public Task<EditedOperationContainer> EditOperationAmount(long id, decimal newAmount, CancellationToken cancellationToken);

    public Task<EditedOperationContainer> EditOperationBankAccountId(long id, long newAccountId, CancellationToken cancellationToken);

    public Task<EditedOperationContainer> EditOperationCategoryId(long id, long newCategoryId, CancellationToken cancellationToken);

    public Task<long> DeleteOperation(long id, CancellationToken cancellationToken);

    public Task<IReadOnlyList<OperationModel>> GetOperations(CancellationToken cancellationToken);

    public Task<IReadOnlyList<OperationCategoryModel>> GetOperationByBankAccountId(long bankAccountId,
        CancellationToken cancellationToken);
}