using Dapper;
using Npgsql;
using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Domain.Exceptions;
using SD.FinancialAccounting.Domain.Models.Enums;

namespace SD.FinancialAccounting.Infrastructure.Dal.Repositories;

public class CategoriesRepository : BaseRepository, ICategoriesRepository
{
    public CategoriesRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<IReadOnlyList<CategoryEntity>> AddCategories(CategoryEntity[] entities,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
INSERT INTO operation_categories (name, type)
    SELECT name, type
    FROM UNNEST(@Categories::operation_category_v1[])
    RETURNING *;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            Categories = entities
        };

        var createdEntities = await connection.QueryAsync<CategoryEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        return createdEntities.ToList();
    }

    public async Task<IReadOnlyList<CategoryEntity>> QueryAllCategories(CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM operation_categories;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<CategoryEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }

    public async Task<CategoryEntity> UpdateCategoryName(long id, string newName, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE operation_categories set
    name = @Name
    WHERE id = @CategoryId
    returning *;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            CategoryId = id,
            Name = newName
        };

        var editedEntities = await connection.QueryAsync<CategoryEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var editedEntitiesList = editedEntities.ToList();
        if (editedEntitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Category not found.");
        }

        return editedEntitiesList[0];
    }

    public async Task<CategoryEntity> UpdateCategoryType(long id, OperationCategoryType newType,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE operation_categories SET
    type = bulk.type
    FROM UNNEST(@Categories::operation_category_v1[]) as bulk
    WHERE operation_categories.id = bulk.id
    returning *;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        CategoryEntity[] abuseDapperList = [new() { Id = id, Type = newType }];
        var sqlParameters = new
        {
            Categories = abuseDapperList
        };

        var editedEntities = await connection.QueryAsync<CategoryEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var editedEntitiesList = editedEntities.ToList();
        if (editedEntitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Category not found.");
        }

        return editedEntitiesList[0];
    }

    public async Task DeleteCategory(long id, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
DELETE FROM operation_categories
       WHERE id = @CategoryId
        returning *;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            CategoryId = id,
        };

        var deletedEntities = await connection.QueryAsync<CategoryEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (deletedEntities.ToList().Count == 0)
        {
            throw new EntityNotFoundException("Category not found.");
        }
    }
}