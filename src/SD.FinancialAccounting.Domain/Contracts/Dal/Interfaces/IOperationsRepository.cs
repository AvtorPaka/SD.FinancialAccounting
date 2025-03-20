using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;

namespace SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;

public interface IOperationsRepository: IDbRepository
{
    public Task<long[]> AddOperations(OperationEntity[] entities, CancellationToken cancellationToken);

    public Task<IReadOnlyList<OperationViewEntity>> QueryOperations(CancellationToken cancellationToken);

    public Task<IReadOnlyList<OperationViewEntity>> QueryOperationsByAccounts(long[] accountIds,
        CancellationToken cancellationToken);

    public Task<OperationViewEntity> QueryOperationById(long id, CancellationToken cancellationToken);
    
    public Task<OperationEntity> DeleteOperation(long id, CancellationToken cancellationToken);

    public Task<OperationEntity> UpdateOperationDescription(long id, string newDescription, CancellationToken cancellationToken);

    public Task<OperationEntity> UpdateOperationAmount(long id, decimal newAmount, CancellationToken cancellationToken);

    public Task<OperationEntity> UpdateOperationAccountId(long id, long newAccountId, CancellationToken cancellationToken);

    public Task<OperationEntity> UpdateOperationCategoryId(long id, long newCategoryId, CancellationToken cancellationToken);
}