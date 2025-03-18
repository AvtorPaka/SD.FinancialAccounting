using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;

namespace SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;

public interface IOperationsRepository: IDbRepository
{
    public Task<long[]> AddOperations(OperationEntity[] entities, CancellationToken cancellationToken);

    public Task<IReadOnlyList<OperationViewEntity>> QueryOperations(CancellationToken cancellationToken);

    public Task<IReadOnlyList<OperationViewEntity>> QueryOperationsByAccount(long accountId,
        CancellationToken cancellationToken);

    public Task<OperationViewEntity> QueryOperationById(long id, CancellationToken cancellationToken);

    public Task DeleteOperation(long id, CancellationToken cancellationToken);

    public Task UpdateOperationDescription(long id, string newDescription, CancellationToken cancellationToken);

    public Task UpdateOperationAmount(long id, decimal newAmount, CancellationToken cancellationToken);

    public Task UpdateOperationAccountId(long id, long newAccountId, CancellationToken cancellationToken);

    public Task UpdateOperationCategoryId(long id, long newCategoryId, CancellationToken cancellationToken);
}