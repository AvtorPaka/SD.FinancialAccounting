using FluentMigrator;

namespace SD.FinancialAccounting.Infrastructure.Dal.Migrations;

[Migration(version: 202503186, TransactionBehavior.Default)]
public class AddOperationCategoryType: Migration {
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'operation_category_v1') THEN
            CREATE TYPE operation_category_v1 as
            (
                id      bigint,
                type    operation_category_type,
                name    varchar(50)
            );
        END IF;
    END
$$;
";
        
        Execute.Sql(sql);
    }

    public override void Down()
    {
        const string sql = @"
DO $$
    BEGIN
        DROP TYPE IF EXISTS operation_category_v1;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}