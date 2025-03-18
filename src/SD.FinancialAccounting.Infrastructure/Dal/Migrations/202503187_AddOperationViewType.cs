using FluentMigrator;

namespace SD.FinancialAccounting.Infrastructure.Dal.Migrations;

[Migration(version: 202503187, TransactionBehavior.Default)]
public class AddOperationViewType: Migration {
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'operation_view_v1') THEN
            CREATE TYPE operation_view_v1 as
            (
                operation_id        bigint,
                bank_account_id     bigint,
                category_id         bigint,
                category_type       integer,
                category_name       varchar(50),
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
        DROP TYPE IF EXISTS operation_view_v1;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}