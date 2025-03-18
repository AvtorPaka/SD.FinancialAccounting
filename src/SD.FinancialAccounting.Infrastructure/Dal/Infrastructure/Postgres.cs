using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Npgsql.NameTranslation;
using SD.FinancialAccounting.Domain.Contracts.Dal.Entities;
using SD.FinancialAccounting.Domain.Models.Enums;
using SD.FinancialAccounting.Infrastructure.Configuration.Models;

namespace SD.FinancialAccounting.Infrastructure.Dal.Infrastructure;

internal static class Postgres
{
    private static readonly INpgsqlNameTranslator Translator = new NpgsqlSnakeCaseNameTranslator();

    public static void ConfigureTypeMapOptions()
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    
    public static void AddDataSource(IServiceCollection services, PostgreConnectionOptions connectionOptions)
    {
        services.AddNpgsqlDataSource(
            connectionString: connectionOptions.ConnectionString,
            builder =>
            {
                builder.MapComposite<BankAccountEntity>("bank_account_v1", Translator);
                builder.MapComposite<CategoryEntity>("operation_category_v1", Translator);
                builder.MapComposite<OperationEntity>("operation_v1", Translator);
                builder.MapComposite<OperationViewEntity>("operation_view_v1", Translator);
                builder.MapEnum<OperationCategoryType>("operation_category_type");
            }
        );
    }

    public static void AddMigrations(IServiceCollection services, PostgreConnectionOptions connectionOptions)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(r => r
                .AddPostgres()
                .WithGlobalConnectionString(s => connectionOptions.ConnectionString)
                .ScanIn(typeof(Postgres).Assembly).For.Migrations()
            );
    }
}