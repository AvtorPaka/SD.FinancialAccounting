using Dapper;
using Npgsql;
using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Exceptions;

namespace SD.FinancialAccounting.Infrastructure.Dal.Repositories;

public class OperationsRepository : BaseRepository, IOperationsRepository
{
    public OperationsRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<long[]> AddOperations(OperationEntity[] entities, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
INSERT INTO operations (bank_account_id, category_id, amount, date, description) 
    SELECT bank_account_id, category_id, amount, date, description
    FROM UNNEST(@Operations::operation_v1[])
    RETURNING id;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            Operations = entities
        };

        IEnumerable<long> createdIds;

        try
        {
            createdIds = await connection.QueryAsync<long>(
                new CommandDefinition(
                    commandText: sqlQuery,
                    parameters: sqlParameters,
                    cancellationToken: cancellationToken
                )
            );
        }
        catch (NpgsqlException ex)
        {
            if (ex.SqlState == "23503")
            {
                throw new EntityNotFoundException("Foreign key not found.");
            }

            throw;
        }

        return createdIds.ToArray();
    }

    public async Task<IReadOnlyList<OperationViewEntity>> QueryOperations(CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM operations_views;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<OperationViewEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }

    public async Task<IReadOnlyList<OperationViewEntity>> QueryOperationsByAccounts(long[] accountIds,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM operations_views
    WHERE bank_account_id = ANY(@AccountIds);
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            AccountIds = accountIds
        };

        var entities = await connection.QueryAsync<OperationViewEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }

    public async Task<OperationViewEntity> QueryOperationById(long id, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM operations_views WHERE operation_id = @OperationId;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            OperationId = id
        };

        var entities = await connection.QueryAsync<OperationViewEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var entitiesList = entities.ToList();
        if (entitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Operation not found.");
        }

        return entitiesList[0];
    }


    public async Task<OperationEntity> DeleteOperation(long id, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
DELETE FROM operations WHERE id = @OperationId returning *;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            OperationId = id
        };

        var deletedOperations = await connection.QueryAsync<OperationEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var deleteOperationsList = deletedOperations.ToList();
        if (deleteOperationsList.Count == 0)
        {
            throw new EntityNotFoundException("Operation not found.");
        }

        return deleteOperationsList[0];
    }

    public async Task<OperationEntity> UpdateOperationDescription(long id, string newDescription, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE operations set
    description = @Description
    WHERE id = @OperationId
    returning *;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            OperationId = id,
            Description = newDescription
        };

        var editedEntities = await connection.QueryAsync<OperationEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var editedEntitiesList = editedEntities.ToList();
        if (editedEntitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Operation not found.");
        }
        
        return editedEntitiesList[0];
    }

    public async Task<OperationEntity> UpdateOperationAmount(long id, decimal newAmount, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE operations set
    amount = @Amount
    WHERE id = @OperationId
    returning *;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            OperationId = id,
            Amount = newAmount
        };

        var editedEntities = await connection.QueryAsync<OperationEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var editedEntitiesList = editedEntities.ToList();
        if (editedEntitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Operation not found.");
        }
        
        return editedEntitiesList[0];
    }

    public async Task<OperationEntity> UpdateOperationAccountId(long id, long newAccountId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE operations set
    bank_account_id = @BankAccountId
    WHERE id = @OperationId
    returning *;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            OperationId = id,
            BankAccountId = newAccountId
        };

        IEnumerable<OperationEntity> editedEntities;
        try
        {
            editedEntities = await connection.QueryAsync<OperationEntity>(
                new CommandDefinition(
                    commandText: sqlQuery,
                    parameters: sqlParameters,
                    cancellationToken: cancellationToken
                )
            );
        }
        catch (NpgsqlException ex)
        {
            if (ex.SqlState == "23503")
            {
                throw new ForeignKeyNotFoundException("Foreign key not found.");
            }

            throw;
        }

        var editedEntitiesList = editedEntities.ToList();
        if (editedEntitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Operation not found.");
        }

        return editedEntitiesList[0];
    }

    public async Task<OperationEntity> UpdateOperationCategoryId(long id, long newCategoryId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE operations set
    category_id = @CategoryId
    WHERE id = @OperationId
    returning *;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            OperationId = id,
            CategoryId = newCategoryId
        };

        IEnumerable<OperationEntity> editedEntities;
        try
        {
            editedEntities = await connection.QueryAsync<OperationEntity>(
                new CommandDefinition(
                    commandText: sqlQuery,
                    parameters: sqlParameters,
                    cancellationToken: cancellationToken
                )
            );
        }
        catch (NpgsqlException ex)
        {
            if (ex.SqlState == "23503")
            {
                throw new ForeignKeyNotFoundException("Foreign key not found.");
            }

            throw;
        }

        var editedEntitiesList = editedEntities.ToList();
        if (editedEntitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Operation not found.");
        }

        return editedEntitiesList[0];
    }
}