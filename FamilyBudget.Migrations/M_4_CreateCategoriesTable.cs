using FluentMigrator;

namespace FamilyBudget.Migrations;

[Migration(4)]
public class M_4_CreateCategoriesTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("categories")
            .WithColumn("index").AsInt64().Identity()
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString(50).NotNullable();
    }
}