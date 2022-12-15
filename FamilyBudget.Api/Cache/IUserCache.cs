using FamilyBudget.Common.Models.Data;

namespace FamilyBudget.Api.Cache;

public interface IUserCache
{
    User? GetUser(Guid id);
    User? GetFlushedUser(Guid id);
    void FlushCacheByUserId(Guid id);
}