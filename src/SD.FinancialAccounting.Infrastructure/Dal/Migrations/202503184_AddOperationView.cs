using FluentMigrator;

namespace SD.FinancialAccounting.Infrastructure.Dal.Migrations;

[Migration(version: 202503184, TransactionBehavior.Default)]
public class AddOperationView: Migration {
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
            CREATE VIEW operations_views AS
                SELECT
                  op.id AS operation_id,
                  op.bank_account_id,
                  cat.id AS category_id,
                  cat.type AS category_type,
                  cat.name AS category_name,
                  op.amount,
                  op.date,
                  op.description
                FROM operations AS op
                INNER JOIN operation_categories AS cat ON op.category_id = cat.id;
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
        DROP VIEW IF EXISTS operations_views;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}