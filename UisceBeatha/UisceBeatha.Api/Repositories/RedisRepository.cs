using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using UisceBeatha.Api.Repositories.Interfaces;

namespace UisceBeatha.Api.Repositories;

public class RedisRepository(IDistributedCache cache) : IRedisRepository
{
    public async Task SaveAsync(string key, string value, TimeSpan ttl, string prefix = "")
    {
        var namespacedKey = $"{prefix}{key}";
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        };
        await cache.SetStringAsync(namespacedKey, value, options);
    }

    public async Task<string?> GetAsync(string key, string prefix = "")
    {
        var namespacedKey = $"{prefix}{key}";
        return await cache.GetStringAsync(namespacedKey);
    }

    public async Task<bool> DeleteAsync(string key, string prefix = "")
    {
        var namespacedKey = $"{prefix}{key}";
        await cache.RemoveAsync(namespacedKey);
        return true;
    }

    public async Task<bool> ExistsAsync(string key, string prefix = "")
    {
        var value = await GetAsync(key, prefix);
        return value != null;
    }
}