using AutoFixture;
using AutoMapper;
using FamilyBudget.Api.AutoMapper;
using FamilyBudget.Api.Cache;
using FamilyBudget.Common.Models.Configuration;
using FamilyBudget.Common.Tools;
using FamilyBudget.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyBudget.Tests.Common;

public class TestHelper
{
    private readonly DatabaseContext _databaseContext;
    private readonly IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;
    private readonly IUserCache _userCache;

    public TestHelper()
    {
        // Auto Fixture
        var fixture = new Fixture();
        
        // Database Context
        var builder = new DbContextOptionsBuilder<DatabaseContext>();
        builder.UseInMemoryDatabase(fixture.Create<string>());
        
        var dbContextOptions = builder.Options;
        _databaseContext = new DatabaseContext(dbContextOptions);
        _databaseContext.Database.EnsureDeleted();
        _databaseContext.Database.EnsureCreated();

        // JWT Utils
        var jwtConfiguration = new JwtConfiguration(fixture.Create<string>());

        _jwtUtils = new JwtUtils(jwtConfiguration);
        
        // Auto Mapper
        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = config.CreateMapper();
        
        // User Cache
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        var memoryCache = serviceProvider.GetService<IMemoryCache>();

        _userCache = new UserCache(memoryCache, _databaseContext);
    }

    public DatabaseContext GetContext()
    {
        return _databaseContext;
    }

    public IJwtUtils GetJwtUtils()
    {
        return _jwtUtils;
    }

    public IMapper GetMapper()
    {
        return _mapper;
    }

    public IUserCache GetCache()
    {
        return _userCache;
    }
}