using FamilyBudget.Common.Models.Data;
using FamilyBudget.Data;
using Microsoft.Extensions.Caching.Memory;
#pragma warning disable CS8600

namespace FamilyBudget.Api.Cache;

public class UserCache : IUserCache
{
    private const string CachePrefix = "user";
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;
    private readonly IMemoryCache _memoryCache;
    private readonly DatabaseContext _databaseContext;

    public UserCache(IMemoryCache memoryCache, DatabaseContext databaseContext)
    {
        _memoryCache = memoryCache;
        _databaseContext = databaseContext;
        _cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
            .SetPriority(CacheItemPriority.Normal)
            .SetSize(1024);
    }

    public User? GetUser(Guid id)
    {
        var cacheKey = $"{CachePrefix}-{id.ToString()}";
        var isUserInCache = _memoryCache.TryGetValue(cacheKey, out User user);
        if (isUserInCache)
        {
            return user;
        }

        user = _databaseContext.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
        {
            return null;
        }
        
        _memoryCache.Set(cacheKey, user, _cacheEntryOptions);
        return user;
    }

    public User? GetFlushedUser(Guid id)
    {
        var cacheKey = $"{CachePrefix}-{id.ToString()}";
        var user = _databaseContext.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
        {
            return null;
        }
        
        _memoryCache.Set(cacheKey, user, _cacheEntryOptions);
        return user;
    }

    public void FlushCacheByUserId(Guid id)
    {
        var cacheKey = $"{CachePrefix}-{id.ToString()}";
        _memoryCache.Remove(cacheKey);
    }
}