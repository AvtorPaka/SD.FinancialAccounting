using FluentMigrator;

namespace SD.FinancialAccounting.Infrastructure.Dal.Migrations;

[Migration(version:202503182, TransactionBehavior.Default)]
public class AddOperationTypeEnum: Migration {
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        CREATE TYPE operation_category_type AS ENUM ('income', 'expense');
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
        DROP TYPE IF EXISTS operation_category_type;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}