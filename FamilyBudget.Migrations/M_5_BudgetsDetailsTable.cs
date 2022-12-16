using System.Data;
using FluentMigrator;

namespace FamilyBudget.Migrations;

[Migration(5)]
public class M5_BudgetDetailsTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("budget_details")
            .WithColumn("index").AsInt64().Identity()
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("budget_id").AsGuid().NotNullable()
            .WithColumn("amount").AsDouble().NotNullable()
            .WithColumn("description").AsString(100).Nullable()
            .WithColumn("user_id").AsGuid().NotNullable()
            .WithColumn("category_id").AsGuid().NotNullable()
            .WithColumn("is_income").AsBoolean().NotNullable();

        Create.ForeignKey()
            .FromTable("budget_details").ForeignColumn("budget_id")
            .ToTable("budgets").PrimaryColumn("id").OnDelete(Rule.Cascade);
        
        Create.ForeignKey()
            .FromTable("budget_details").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id").OnDelete(Rule.None);
    }
}