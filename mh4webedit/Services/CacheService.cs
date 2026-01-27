using mh4edit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CacheService(IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
    {
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetItem(string key, object value)
    {
        var opts = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // sliding window
        _cache.Set(key, value, opts);
    }

    public T? GetItem<T>(string key) where T : class
    {
        if(_cache.TryGetValue(key, out T value))
        {
            return value;
        }
        return null;
    }

    public bool HasSavegame()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return false;

        MonHunSave sav = GetItem<MonHunSave>($"Savegame-{session.Id}");
        return sav != null;
    }

    public MonHunSave? GetSavegame()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return null;
        return GetItem<MonHunSave>($"Savegame-{session.Id}");
    }

    public void SetSavegame(MonHunSave savegame)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) throw new InvalidOperationException("No active HTTP session available.");
        SetItem($"Savegame-{session.Id}", savegame);
    }
}