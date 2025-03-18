using System.Data;
using FluentMigrator;
using FluentMigrator.Postgres;

namespace SD.FinancialAccounting.Infrastructure.Dal.Migrations;

[Migration(version:202503182, TransactionBehavior.Default)]
public class InitSchema: Migration {
    public override void Up()
    {
        Create.Table("bank_accounts")
            .WithColumn("id").AsInt64().PrimaryKey("bank_accounts_pk").Identity()
            .WithColumn("name").AsString(50).NotNullable()
            .WithColumn("balance").AsDecimal(19, 5).NotNullable().WithDefaultValue(0);

        Create.Table("operation_categories")
            .WithColumn("id").AsInt64().PrimaryKey("operation_categories_pk").Identity()
            .WithColumn("type").AsInt32().NotNullable()
            .WithColumn("name").AsString(50).NotNullable();

        Create.Table("operations")
            .WithColumn("id").AsInt64().PrimaryKey("operations_pk").Identity()
            .WithColumn("bank_account_id").AsInt64().NotNullable()
            .WithColumn("category_id").AsInt64().NotNullable()
            .WithColumn("amount").AsDecimal(19, 5).NotNullable().WithDefaultValue(0)
            .WithColumn("date").AsDateTimeOffset().NotNullable()
            .WithColumn("description").AsString(255).NotNullable();

        Create.ForeignKey("FK_operations_bankAccounts")
            .FromTable("operations").ForeignColumn("bank_account_id")
            .ToTable("bank_accounts").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);
        
        Create.ForeignKey("FK_operations_categories")
            .FromTable("operations").ForeignColumn("category_id")
            .ToTable("operation_categories").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);

    }

    public override void Down()
    {
        Delete.Table("operations");
        Delete.Table("operation_categories");
        Delete.Table("bank_accounts");
    }
}