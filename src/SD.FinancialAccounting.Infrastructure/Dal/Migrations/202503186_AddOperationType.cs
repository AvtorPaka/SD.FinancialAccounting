using FluentMigrator;

namespace SD.FinancialAccounting.Infrastructure.Dal.Migrations;

[Migration(version:202503186, TransactionBehavior.Default)]
public class AddOperationType: Migration {
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'operation_v1') THEN
            CREATE TYPE operation_v1 as
            (
                id                  bigint,
                bank_account_id     bigint,
                category_id         bigint,
                amount              numeric(19, 5),
                date                timestamp with time zone,
                description         varchar(255)
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
        DROP TYPE IF EXISTS operation_v1;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}