using System.Data;
using FluentMigrator;

namespace FamilyBudget.Migrations;

[Migration(3)]
public class M_3_CreateBudgetsUsersTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("budgets_users")
            .WithColumn("index").AsInt64().Identity()
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("budget_id").AsGuid().NotNullable()
            .WithColumn("user_id").AsGuid().NotNullable();
        
        Create.ForeignKey()
            .FromTable("budgets_users").ForeignColumn("budget_id")
            .ToTable("budgets").PrimaryColumn("id").OnDelete(Rule.None);
        
        Create.ForeignKey()
            .FromTable("budgets_users").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id").OnDelete(Rule.None);
    }
}