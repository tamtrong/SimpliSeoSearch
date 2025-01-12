using Microsoft.Extensions.Caching.Memory;

namespace SimpliSeoSearch.Core.Cache;

public class MemoryCacheService: ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public void Set(string key, object value, TimeSpan expiry)
    {
        _memoryCache.Set(key, value, expiry);
    }

    public object? Get(string key)
    {
        return _memoryCache.TryGetValue(key, out var value) ? value : default;
    }
}