using Dapper;
using Npgsql;
using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Exceptions;

namespace SD.FinancialAccounting.Infrastructure.Dal.Repositories;

public class BankAccountRepository : BaseRepository, IBankAccountRepository
{
    public BankAccountRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<IReadOnlyList<BankAccountEntity>> QueryAllAccounts(CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM bank_accounts;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<BankAccountEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }

    public async Task<IReadOnlyList<BankAccountEntity>> QueryAccountsByIds(long[] ids,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM bank_accounts WHERE id = ANY(@Ids);
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            Ids = ids
        };

        var entities = await connection.QueryAsync<BankAccountEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var entityList = entities.ToList();
        if (entityList.Count == 0)
        {
            throw new EntityNotFoundException("Bank accounts not found.");
        }

        return entityList;
    }

    public async Task<IReadOnlyList<BankAccountEntity>> AddAccounts(BankAccountEntity[] entities,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
INSERT INTO bank_accounts (name, balance)
    SELECT name, balance 
    FROM UNNEST(@Accounts)
    returning *;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            Accounts = entities
        };

        var addedEntities = await connection.QueryAsync<BankAccountEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        return addedEntities.ToList();
    }

    public async Task DeleteAccount(long id, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
DELETE FROM bank_accounts WHERE id = @AccountId returning id;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            AccountId = id
        };

        var deletedAccountId = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (deletedAccountId.FirstOrDefault() != id)
        {
            throw new EntityNotFoundException("Bank account not found.");
        }
    }

    public async Task<long[]> UpdateAccountsBalance(BankAccountEntity[] entities, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE bank_accounts SET 
    balance = bulk.balance
    FROM UNNEST(@Accounts) as bulk
    WHERE bank_accounts.id = bulk.id
    returning bank_accounts.id;
";

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            Accounts = entities
        };

        var editedIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var editedIdsList = editedIds.ToArray();
        if (editedIdsList.Length != entities.Length)
        {
            throw new EntityNotFoundException("Several accounts not found.");
        }

        return editedIdsList;
    }

    public async Task<IReadOnlyList<BankAccountEntity>> UpdateAccountName(BankAccountEntity[] entities,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE bank_accounts SET 
    name = bulk.name
    FROM UNNEST(@Accounts) as bulk
    WHERE bank_accounts.id = bulk.id
    returning *;
";

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            Accounts = entities
        };

        var editedEntities = await connection.QueryAsync<BankAccountEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var editedEntitiesList = editedEntities.ToList();
        if (editedEntitiesList.Count != entities.Length)
        {
            throw new EntityNotFoundException("Several accounts not found.");
        }

        return editedEntitiesList;
    }
}