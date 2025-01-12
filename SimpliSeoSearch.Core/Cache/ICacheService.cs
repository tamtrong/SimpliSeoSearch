namespace SimpliSeoSearch.Core.Cache;

public interface ICacheService  
{
    void Set(string key, object value, TimeSpan expiry);
    object? Get(string key);
}