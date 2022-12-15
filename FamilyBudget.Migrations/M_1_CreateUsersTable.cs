using FluentMigrator;

namespace FamilyBudget.Migrations;

[Migration(1)]
public class M_1_CreateUsersTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("index").AsInt64().Identity()
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("login").AsString(50).NotNullable()
            .WithColumn("password").AsString(250).NotNullable()
            .WithColumn("create_time").AsDateTime().NotNullable()
            .WithColumn("last_login_time").AsDateTime().Nullable()
            .WithColumn("authentication_hash").AsString(250).Nullable();
    }
}