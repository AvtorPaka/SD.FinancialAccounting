using FluentMigrator;

namespace SD.FinancialAccounting.Infrastructure.Dal.Migrations;

[Migration(version: 2025031804, TransactionBehavior.Default)]
public class AddBankAccountType: Migration {
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'bank_account_v1') THEN
            CREATE TYPE bank_account_v1 as
            (
                id      bigint,
                name    varchar(50),
                balance numeric(19, 5)
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
        DROP TYPE IF EXISTS bank_account_v1;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}