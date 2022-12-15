using FamilyBudget.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyBudget.Data;

public class Migrator
{
    private readonly IServiceProvider _provider;

    public Migrator(string connectionString)
    {
        _provider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(MigrationScanner).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }
    
    
    /// <summary>
    /// Update the database
    /// </summary>
    public void UpdateDatabase()
    {
        // Instantiate the runner
        var runner = _provider.GetRequiredService<IMigrationRunner>();

        // Execute the migrations
        runner.MigrateUp();
    }

    public void RollbackDatabase(int steps)
    {
        // Instantiate the runner
        var runner = _provider.GetRequiredService<IMigrationRunner>();

        // Execute the migrations
        runner.Rollback(steps);
    }
}