using FluentMigrator;

namespace FamilyBudget.Migrations;

[Migration(2)]
public class M_2_CreateBudgetsTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("budgets")
            .WithColumn("index").AsInt64().Identity()
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString(50).NotNullable()
            .WithColumn("description").AsString(250).Nullable()
            .WithColumn("create_time").AsDateTime().NotNullable();
    }
}