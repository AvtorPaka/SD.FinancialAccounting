using Npgsql;
using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;

namespace SD.FinancialAccounting.Infrastructure.Dal.Repositories;

public class OperationsRepository: BaseRepository, IOperationsRepository
{
    public OperationsRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<long[]> AddOperations(OperationEntity[] entities, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<OperationViewEntity>> QueryOperations(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<OperationViewEntity>> QueryOperationsByAccount(long accountId, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<OperationViewEntity> QueryOperationById(long id, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task DeleteOperation(long id, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task UpdateOperationDescription(long id, string newDescription, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task UpdateOperationAmount(long id, decimal newAmount, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task UpdateOperationAccountId(long id, long newAccountId, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task UpdateOperationCategoryId(long id, long newCategoryId, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}