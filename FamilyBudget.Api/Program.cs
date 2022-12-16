using FamilyBudget.Api.Authorization;
using FamilyBudget.Api.Cache;
using FamilyBudget.Common.Models.Configuration;
using FamilyBudget.Common.Tools;
using FamilyBudget.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Family Budget Api", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Dependency Injection
var configuration = builder.Configuration;
var connectionStrings = configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
var jwtConfiguration = configuration.GetSection("JWTConfiguration").Get<JwtConfiguration>();

builder.Services.AddSingleton(jwtConfiguration!);

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

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// User Cache
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IUserCache, UserCache>();

// JWT Utils
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();