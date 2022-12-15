using FamilyBudget.Common.Models.Configuration;
using FamilyBudget.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
var configuration = builder.Configuration;
var connectionStrings = configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();

#if DEBUG
var connectionString = connectionStrings!.Local;
#else
var connectionString = connectionStrings!.Container;
#endif

// Entity Framework
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(connectionString));

// Migrator
var migrator = new Migrator(connectionString);
migrator.UpdateDatabase();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();